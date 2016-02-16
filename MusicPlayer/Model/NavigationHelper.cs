using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MusicPlayer.Model
{
    public class NavigationHelper
    {
        Stack<object> PageStack;
        public static NavigationHelper Instance {get; set; }
        private NavigationHelper()
        {
            PageStack = new Stack<object>();
        }

        public static NavigationHelper GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NavigationHelper();
            }
            return Instance;
        }

        public void Add(object page)
        {
            PageStack.Push(page);
        }

        public object Pop()
        {
            return PageStack.Pop();
        }

        public int Count()
        {
            return PageStack.Count;
        }
    }
}
