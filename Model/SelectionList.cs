using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ColorSelectorSample.Model;

/// <summary>
/// Provides a list with a selected value.
/// </summary>
/// <typeparam name="T">The type of object contained in the list.</typeparam>
public class SelectionList<T> : INotifyPropertyChanged, IReadOnlyList<T>, INotifyCollectionChanged
{
    T _selectedValue;
    readonly List<T> _values;

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="values">The <typeparamref name="T"/> values to populate the list.</param>
    public SelectionList(IEnumerable<T> values)
    {
        _values = new List<T>(values);
    }

    /// <summary>
    /// Sorts the items in the list.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{T}"/> to use to sort the list.</param>
    public void Sort(IComparer<T> comparer)
    {
        _values.Sort(comparer);
        CollectionChanged?.Invoke(this, CollectionResetEventArgs);
    }

    #region Properties

    /// <summary>
    /// Gets or sets the selected <typeparamref name="T"/>.
    /// </summary>
    public T SelectedItem
    {
        get => _selectedValue;
        set
        {
            if (!object.ReferenceEquals(_selectedValue, value))
            {
                _selectedValue = value;
                PropertyChanged?.Invoke(this, SelectedItemChangedEventArgs);
            }
        }
    }

    /// <summary>
    /// Gets the number of items in the list.
    /// </summary>
    public int Count
    {
        get => _values.Count;
    }

    /// <summary>
    /// Gets the item at the specified <paramref name="index"/>
    /// </summary>
    /// <param name="index">The index of the item to get.</param>
    /// <returns>The <typeparamref name="T"/> at the specified <paramref name="index"/>.</returns>
    public T this[int index]
    {
        get => _values[index];
        protected set => _values[index] = value;
    }

    #endregion Properties

    #region IEnumerable

    /// <summary>
    /// Gets an <see cref="IEnumerator{T}"/>  for enumerating the items in the list.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{T}"/>  for enumerating the items in the list.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    /// <summary>
    /// Gets an <see cref="IEnumerator"/>  for enumerating the items in the list.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/>  for enumerating the items in the list.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    #endregion IEnumerable

    /// <summary>
    /// Occurs when the collection changes.
    /// </summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    /// <summary>
    /// Occurs when a property changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #region Cached EventArgs

    /// <summary>
    /// Defines the <see cref="NotifyCollectionChangedEventArgs"/> used when the collection is reset.
    /// </summary>
    static readonly public NotifyCollectionChangedEventArgs CollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="SelectedItem"/> changes.
    /// </summary>
    static public readonly PropertyChangedEventArgs SelectedItemChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedItem));

    #endregion Cached PropertyChangedEventArgs

}
