using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RevitAddIns
{
    public class Helper
    {
        public static BitmapFrame ConvertResourcetoImgSource(string resourcePath, double Pixel)
        {
            BitmapFrame result = null;

            try
            {
                using (Stream resStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(resStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapFrame frame = decoder.Frames[0];
                    result = BitmapFrame.Create(new TransformedBitmap(frame, new ScaleTransform(Pixel/ frame.Width, Pixel/ frame.Height)));
                }    
            }
            catch
            {

            }

            return result;
        }

        public class PropertyChangedBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }


        public class ObservableRangeCollection<T> : ObservableCollection<T>
        {
            /// <summary> 
            /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
            /// </summary> 
            public void AddRange(IEnumerable<T> collection)
            {
                if (collection == null) throw new ArgumentNullException("collection");

                foreach (var i in collection) Items.Add(i);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            /// <summary> 
            /// Removes the first occurence of each item in the specified collection from ObservableCollection(Of T). 
            /// </summary> 
            public void RemoveRange(IEnumerable<T> collection)
            {
                if (collection == null) throw new ArgumentNullException("collection");

                foreach (var i in collection) Items.Remove(i);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            /// <summary> 
            /// Clears the current collection and replaces it with the specified item. 
            /// </summary> 
            public void Replace(T item)
            {
                ReplaceRange(new T[] { item });
            }

            /// <summary> 
            /// Clears the current collection and replaces it with the specified collection. 
            /// </summary> 
            public void ReplaceRange(IEnumerable<T> collection)
            {
                if (collection == null) throw new ArgumentNullException("collection");

                Items.Clear();
                foreach (var i in collection) Items.Add(i);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            /// <summary> 
            /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class. 
            /// </summary> 
            public ObservableRangeCollection()
                : base() { }

            /// <summary> 
            /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class that contains elements copied from the specified collection. 
            /// </summary> 
            /// <param name="collection">collection: The collection from which the elements are copied.</param> 
            /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception> 
            public ObservableRangeCollection(IEnumerable<T> collection)
                : base(collection) { }
        }
    }
}
