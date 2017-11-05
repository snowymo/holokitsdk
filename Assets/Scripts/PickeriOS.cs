#if UNITY_IOS
using System.Runtime.InteropServices;

namespace Zhenyi
{
    internal class PickeriOS : IPicker
    {
        [DllImport("__Internal")]
		private static extern void docpicker_show(string title, string outputFileName, int maxSize);

        public void Show(string title, string outputFileName, int maxSize)
        {
			//Console.Log("in namespace Zhenyi: IPicker: Show()function");
            docpicker_show(title, outputFileName, maxSize);
        }
    }
}
#endif