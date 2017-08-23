﻿using System.Collections.Generic;
using SEMining.Sandbox.Models;

namespace SEMining.Sandbox.Holders
{
    public class SandboxDataHolder : ISandboxDataHolder
    {
        private IEnumerable<Slice> _dataStorage = new List<Slice>();

        public void Add(IEnumerable<Slice> data)
        {
            _dataStorage = data;
        }

        public void Clean()
        {
            _dataStorage = new List<Slice>();
        }

        public IEnumerable<Slice> Get()
        {
            return _dataStorage;
        }
    }
}