using UnityEngine;
namespace Game
{
    public class Calculator
    {
        public int Add(int a, int b) => a + b;

        public int Sub(int a, int b) => a - b;

        public void PrintStuffOut()
        {
            int something = -1;
            Debug.Log(something);
            Debug.Log("123");
            Debug.Log("123");
        }
    }
}
