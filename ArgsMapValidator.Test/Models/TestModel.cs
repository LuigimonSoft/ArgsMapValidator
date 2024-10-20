using System;
using ArgsMapValidator.Attributes;

namespace ArgsMapValidator.Tests.Models
{
    public class TestModel
    {
        [Parameter(0)]
        public string Name { get; set; }

        [Parameter(1)]
        public int Age { get; set; }

        [Parameter(2)]
        public decimal Salary { get; set; }

        [Parameter(3)]
        public string FilePath { get; set; }
    }
}