﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDAI.Helpers
{
    interface IDataGenerationRunner
    {
        public void Run(Action<BiasSetup> action);
    }

    public class BiasRunnerConstant : IDataGenerationRunner
    {
        private readonly int _vars;
        private readonly int _body;
        private readonly int _clause;

        public BiasRunnerConstant(int vars, int body, int clause)
        {
            _body = body;
            _clause = clause;
            _vars = vars;
        }

        public void Run(Action<BiasSetup> action)
        {
            action(new BiasSetup()
            {
                Var = _vars,
                Body = _body,
                Clause = _clause,
            });
        }
    }

    public class BiasRunnerInfinite : IDataGenerationRunner
    {
        private int _minVars;
        private int _forward;

        public BiasRunnerInfinite(int minVars, int forward)
        {
            _minVars = minVars;
            _forward = forward;
        }

        public void Run(Action<BiasSetup> action)
        {
            for (int x = 0; x < _forward; x++)
            {
                for (int y = 0; y < x; y++)
                {
                    for (int z = 0; z < x; z++)
                    {
                        action(new BiasSetup()
                        {
                            Var = x + _minVars,
                            Body = y + 5,
                            Clause = z + 5,
                        });
                    }
                }
            }
        }
    }
}
