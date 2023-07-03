using System.Net.WebSockets;
using System.Text;

namespace HR
{
    public static class Extensions {

        public static ArraySegment<byte> ToArraySegmentBuffer (this string text) {
            return new ArraySegment<byte>(Encoding.ASCII.GetBytes(text));
        }

        public static int? ToInt (this WebSocketCloseStatus? status) {
            return (status == null ? null : (int?)((int)status));
        }

        public static void SafeInvoke (this Action action, Action<Exception> onError = null) {
            if (action != null) {
                try {
                    action.Invoke();
                } catch (Exception ex) {
                    if (onError != null) onError.Invoke(ex);
                }
            }
        }

        public static void SafeInvoke <T1> (this Action<T1> action, T1 arg1, Action<Exception> onError = null) {
            if (action != null) {
                try {
                    action.Invoke(arg1);
                } catch (Exception ex) {
                    if (onError != null) onError.Invoke(ex);
                }
            }
        }

        public static void SafeInvoke <T1, T2> (this Action<T1, T2> action, T1 arg1, T2 arg2, Action<Exception> onError = null) {
            if (action != null) {
                try {
                    action.Invoke(arg1, arg2);
                } catch (Exception ex) {
                    if (onError != null) onError.Invoke(ex);
                }
            }
        }

        public static T WaitForResult <T> (this Task<T> task) {
            try {
                task.Wait();
                return task.Result;

            } catch (Exception) {
                return default(T);
            }
        }

        /** <summary> Returns and removes the element at the given index </summary> */
        public static T Steal <T> (this IList<T> list, int index) {
            T value = list.ElementAt(index);
            list.RemoveAt(index);
            return value;
        }

        /** <summary> Returns and removes the first element </summary> */
        public static T StealFirst <T> (this IList<T> list) {
            if (list.Count <= 0) return default;
            else return list.Steal(0);
        }

        /** <summary> Suspends the current thread while the wait function returns true. The wait function is queried at an interval equal to msTimeout, in milliseconds. </summary> */
        public static void WaitWhile (this object _, int msTimeout, Func<bool> wait) {
            while (wait.Invoke()) {
                Thread.Sleep(msTimeout);
            }
        }
    }
}
