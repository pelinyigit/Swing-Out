using UnityEngine;
using System;

[Serializable]
public class Doubling
{
    [SerializeField] DoublingPrefix prefix;
    [SerializeField] DoublingValue value;

    public DoublingPrefix Prefix { get => prefix; }
    public DoublingValue Value { get => value; }
}

public enum DoublingValue
{
    One = 1, Two = 2, Three = 3
}

public enum DoublingPrefix
{
    Plus = '+', Minus = '-', Divide = '/', Multiply = 'x'
}