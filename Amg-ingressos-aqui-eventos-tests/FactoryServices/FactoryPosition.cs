using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryPosition
    {
        internal static Positions SimplePosition()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(),
                SoldPositions = new List<int>(),
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithSoldPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(),
                SoldPositions = new List<int>() { 1, 2, 3 },
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>() { 4, 5, 6 },
                SoldPositions = new List<int>(),
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositionsAndSoldPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>() { 4, 5, 6 },
                SoldPositions = new List<int>() { 1, 2, 3 },
                TotalPositions = 100,
            };
        }

    }
}