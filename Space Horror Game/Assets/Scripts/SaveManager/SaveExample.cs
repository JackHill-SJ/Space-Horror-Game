using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveManagment.Examples
{

    public class SaveExample : MonoBehaviour
    {
        private SaveDataGeneric<int> intTest = null;
        private SaveDataGeneric<float> floatTest = null;
        private SaveDataGeneric<DateTime> dateTest = null;
        private SaveDataGeneric<ExampleClass> exampleTest = null;

        // Start is called before the first frame update
        void Start()
        {
            intTest = new SaveDataGeneric<int>("int test", 1);
            floatTest = new SaveDataGeneric<float>("float test", 69.87f);
            dateTest = new SaveDataGeneric<DateTime>("date test", DateTime.Now);
            exampleTest = new SaveDataGeneric<ExampleClass>("customclass test", new ExampleClass(4, "haha"));

            Debug.Log("intTest: " + intTest.Value.ToString());
            Debug.Log("float test: " + floatTest.Value.ToString());
            Debug.Log("date test: " + dateTest.Value.ToString());
            Debug.Log("example test: v: " + exampleTest.Value.v.ToString() + " test: " + exampleTest.Value.v.ToString());

            intTest.Value++;
            floatTest.Value = 69 / 420;
            dateTest.Value = new DateTime(2020, 4, 20);
            exampleTest.Value.v = 999;
            exampleTest.Value.test = "hoho";
        }
    }

    [System.Serializable]
    public class ExampleClass
    {
        public float v = 0;
        public string test = "";

        public ExampleClass(float _v, string _test)
        {
            v = _v;
            test = _test;
        }
    }

}