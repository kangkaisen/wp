using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Myprogress
{
    public static class Extensions
    {
        public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element)
        {
            return GetVisualDescendantsAndSelfIterator(element).Skip(1);
        }

        public static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator(DependencyObject element)
        {
            Queue<DependencyObject> remaining = new Queue<DependencyObject>();
            remaining.Enqueue(element);
            while (remaining.Count > 0)
            {
                DependencyObject obj = remaining.Dequeue();
                yield return obj;
                foreach (DependencyObject child in obj.GetVisualChildren())
                {
                    remaining.Enqueue(child);
                }
            }
        }

        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element)
        {
            return GetVisualChildrenAndSelfIterator(element).Skip(1);
        }

        public static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(this DependencyObject element)
        {
            yield return element;

            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(element, i);
            }
        }
    }
}
