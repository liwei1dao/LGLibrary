using System.Collections;

namespace LG
{

    public class CoroutineTask
    {
        public bool Running
        {
            get
            {
                return running;
            }
        }

        public bool Paused
        {
            get
            {
                return paused;
            }
        }
        public delegate void FinishedHandler(CoroutineTask task, bool manual);
        public event FinishedHandler Finished;
        IEnumerator coroutine;
        bool running;
        bool paused;

        public CoroutineTask(IEnumerator c)
        {
            coroutine = c;
            Start();
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void Start()
        {
            running = true;
        }

        public void Stop()
        {
            running = false;
        }

        public IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        running = false;
                    }
                }
            }
        }
    }
}