using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DebugMenu
{
    public abstract class DebugMenuSliderBase : DebugOption
    {
        public string indexer = "";

        public DebugMenuSliderBase(string name)
            : base(name) 
        {
            this.description = IndexerString;
        }

        public virtual void Increment()
        {
            GenerateIndexer();
        }

        public virtual void Decrement()
        {
            GenerateIndexer();
        }

        protected abstract void GenerateIndexer();
        public virtual string IndexerString()
        {
            return indexer;
        }
    }

    public abstract class DebugMenuSliderBase<T> : DebugMenuSliderBase
        where T : IComparable<T>
    {
        public T min;
        public T max;
        public T increment;
        public T val;
        public Action<T> outVal;

        public DebugMenuSliderBase(string name, T min, T max, T increment, Action<T> outVal)
            : this(name, min, max, increment, min, outVal) { }

        public DebugMenuSliderBase(string name, T min, T max, T increment, T val, Action<T> outVal)
            : base(name)
        {
            this.min = min;
            this.max = max;
            this.increment = increment;
            this.val = val;
            this.outVal = outVal;
            GenerateIndexer();
        }
    }

    public class DebugMenuSliderFloat : DebugMenuSliderBase<float>
    {
        public DebugMenuSliderFloat(string name, float min, float max, float increment, Action<float> outVal)
            : base(name, min, max, increment, min, outVal) { }

        public DebugMenuSliderFloat(string name, float min, float max, float increment, float val, Action<float> outVal)
            : base(name, min, max, increment, val, outVal) { }

        public override void Decrement()
        {
            val = Math.Max(min, val - increment);
            outVal(val);
            base.Decrement();
        }

        public override void Increment()
        {
            val = Math.Min(max, val + increment);
            outVal(val);
            base.Increment();
        }

        protected override void GenerateIndexer()
        {
            string outVal = "<";
            float remappedVal = val.Remap(min, max, 0, 10);
            for (int i = 0; i < 11; i++)
            {
                outVal = outVal + ((i == (int)remappedVal) ? " O" : " -");
            }

            indexer = $"({val})" + outVal + $" >";
        }
    }

    public class DebugMenuEnum<T> : DebugMenuSliderBase
        where T : Enum
    {
        int i = 0;
        public Action<T> outVal;
        T[] args;

        public DebugMenuEnum(string name, Action<T> outVal)
            : this(name, default(T), outVal)
        {

        }

        public DebugMenuEnum(string name, T val, Action<T> outVal)
            : base(name)
        {
            args = Utilities.GetEnums<T>().ToArray();
            i = Array.IndexOf(args, val);
            if (i == -1)
            {
                Debug.LogError("couldn't find val in args.");
                i = 0;
            }

            this.outVal = outVal;
            GenerateIndexer();
        }

        public override void Increment()
        {
            i = Math.Min(i + 1, args.Length - 1);
            outVal(args[i]);
            base.Increment();
        }

        public override void Decrement()
        {
            i = Math.Max(0, i - 1);
            outVal(args[i]);
            base.Decrement();
        }

        protected override void GenerateIndexer()
        {
            indexer = $"< {args[i]} >";
        }
    }

    public class DebugMenuSliderInt : DebugMenuSliderBase<int>
    {
        public DebugMenuSliderInt(string name, int min, int max, int increment, Action<int> outVal)
            : base(name, min, max, increment, min, outVal) { }

        public DebugMenuSliderInt(string name, int min, int max, int increment, int val, Action<int> outVal)
            : base(name, min, max, increment, val, outVal) { }

        public override void Decrement()
        {
            val = Math.Max(min, val - increment);
            outVal(val);
            base.Decrement();
        }

        public override void Increment()
        {
            val = Math.Min(max, val + increment);
            outVal(val);
            base.Increment();
        }

        protected override void GenerateIndexer()
        {
            string outVal = "<";
            float remappedVal = ((float)val).Remap(min, max, 0, 10);
            for (int i = 0; i < 11; i++)
            {
                outVal = outVal + ((i == remappedVal) ? " O" : " -");
            }

            indexer = $"({val})" + outVal + $" >";
        }
    }
    
    public class DebugMenuSliderUI : DebugOptionUI
    {
        DebugMenuSliderBase DebugMenuSliderBase;

        public override void UpdateData()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (highlighted && (Controller.GetKeyDown(Controller.Controls.DebugRight) | Controller.GetKey(Controller.Controls.DebugRight, 1.0f)))
            {
                DebugMenuSliderBase.Increment();
            }
            else if (highlighted && (Controller.GetKeyDown(Controller.Controls.DebugLeft) | Controller.GetKey(Controller.Controls.DebugLeft, 1.0f)))
            {
                DebugMenuSliderBase.Decrement();
            }
#endif
            base.UpdateData();
        }

        public void ApplyData(DebugMenuSliderBase debugMenuSliderBase)
        {
            DebugMenuSliderBase = debugMenuSliderBase;
            base.ApplyData(DebugMenuSliderBase);
        }
    }
}