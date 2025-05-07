using System.Collections.Generic;
using System.Collections.Specialized;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Views;
using SL.Services;
using UnityEngine;
using Zenject;

namespace SL.UI.Views.Base
{
    public class CollectionView : CollectionView<string, ItemViewBase>
    {
        protected override void UpdateItem(ItemViewBase view, string itemId)
        {
            view.SetItem(itemId);
        }
    }

    public abstract class CollectionView<TData, TView> : ListCollectionViewBase<TData> where TView : AbstractItemViewBase
    {
        [SerializeField] protected Transform content;
        [SerializeReference] protected TView viewPrefab;

        private IPrefabFactory _prefabFactory;
        private readonly List<TView> _views = new();
        private readonly Queue<TView> _pool = new();

        //public ISelectionGroup Group { get; set; }
        public IReadOnlyList<TView> Views => _views;

        [Inject]
        public void Construct(IPrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
        }

        public void Hide()
        {
            Clear();
        }

        public void Show()
        {
            OnItemsChanged();
        }

        private TView CreateView(TData itemId)
        {
            TView view = default;
            if (_pool.Count > 0)
            {
                view = _pool.Dequeue();
            }
            else
            {
                view = _prefabFactory.InstantiatePrefabForComponent<TView>(viewPrefab.gameObject, content);
                //view.Group = Group;
                OnViewCreated(view);
            }

            view.gameObject.SetActive(true);
            UpdateViewModelInternal(view, itemId);
            return view;
        }

        protected virtual void OnViewCreated<T>(T view) where T : TView { }

        protected abstract void UpdateItem(TView view, TData itemId);

        private void UpdateViewModelInternal(TView view, TData item)
        {
            UpdateItem(view, item);
        }

        private void RemoveView(TView view)
        {
            _pool.Enqueue(view);
            UpdateViewModelInternal(view, default);
            view.gameObject.SetActive(false);
        }

        protected override void Clear()
        {
            _views.ForEach(RemoveView);
            _views.Clear();
        }

        protected override void OnItemRemoved(int index, TData item)
        {
            var view = _views[index];
            _views.RemoveAt(index);
            RemoveView(view);
        }

        protected override void OnItemReplaced(int index, TData oldItem, TData newItem)
        {
            UpdateViewModelInternal(_views[index], newItem);
            Reorder();
        }

        protected override void OnItemAdded(int index, TData item)
        {
            if (index < _views.Count)
            {
                UpdateViewModelInternal(_views[index], item);
            }
            else
            {
                var newView = CreateView(item);
                _views.Add(newView);
            }

            _views[index].transform.SetSiblingIndex(index);
            Reorder();
        }

        private void Reorder()
        {
            for (var i = 0; i < _views.Count; i++)
            {
                _views[i].transform.SetSiblingIndex(i);
            }
        }

        protected override void OnItemsChanged()
        {
            Clear();
            for (var i = 0; i < InternalItems.Count; i++)
            {
                OnItemAdded(i, InternalItems[i]);
            }

            Reorder();
        }

        protected override void OnItemMoved(int oldIndex, int newIndex, TData item)
        {
            (_views[oldIndex], _views[newIndex]) = (_views[newIndex], _views[oldIndex]);
            Reorder();
        }

        protected override void OnDestroy()
        {
            _views.ForEach(a => Destroy(a.gameObject));

            foreach (TView view in _pool)
                Destroy(view.gameObject);

            _views.Clear();
        }
    }

    public abstract class ListCollectionViewBase<T> : UIView
    {
        protected ObservableList<T> InternalItems;

        public ObservableList<T> Items
        {
            get { return InternalItems; }
            set
            {
                if (InternalItems == value)
                    return;

                if (InternalItems != null)
                    InternalItems.CollectionChanged -= OnCollectionChanged;

                InternalItems = value;
                OnItemsChanged();

                if (InternalItems != null)
                    InternalItems.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnItemAdded(eventArgs.NewStartingIndex, (T)eventArgs.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnItemRemoved(eventArgs.OldStartingIndex, (T)eventArgs.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    OnItemReplaced(eventArgs.OldStartingIndex, (T)eventArgs.OldItems[0], (T)eventArgs.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
                case NotifyCollectionChangedAction.Move:
                    OnItemMoved(eventArgs.OldStartingIndex, eventArgs.NewStartingIndex, (T)eventArgs.NewItems[0]);
                    break;
            }
        }

        protected abstract void OnItemMoved(int oldIndex, int newIndex, T item);

        protected abstract void Clear();

        protected abstract void OnItemRemoved(int index, T item);

        protected abstract void OnItemReplaced(int index, T oldItem, T newItem);

        protected abstract void OnItemAdded(int index, T item);

        protected abstract void OnItemsChanged();
    }
}