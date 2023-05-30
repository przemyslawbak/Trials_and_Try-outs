using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace List_Comparer
{
    public class Patterns
    {
        private VariablesModel GetVariables(List<OhlcvObject> _data)
        {
            return new VariablesModel()
            {
                _minCandleSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxShortCandleSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _minCandleDifference = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxDojiShadowSizes = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxDojiBodySize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _minCandleShadowSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxShadowSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxPriceDifference = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxCloseDifference = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxCandleBodySize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxCandleShadowSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxDifference = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxShadowChange = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxCloseHighChange = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _maxCandleSize = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 4,
                _maxOpenDifference = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
                _inBarMaxChange = _data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 4,
            };
        }

        public List<OhlcvObject> Bearish2Crows(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close <= _data[i - 0].Open && _data[i - 0].Open <= _data[i - 1].Open && _data[i - 2].Open <= _data[i - 0].Close && _data[i - 0].Close <= _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3BlackCrows(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < _data[i - 2].Close && _data[i - 2].Close <= _data[i - 1].Open && _data[i - 1].Open <= _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3InsideDown(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3OutsideDown(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < variables._maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3LineStrike(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open > _data[i - 3].Close && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Open && _data[i - 1].Open < _data[i - 2].Open && _data[i - 1].Close < _data[i - 2].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                        {
                            if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open <= _data[i - 1].Close && _data[i - 0].Close >= _data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishAdvanceBlock(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && ((100 + variables._minCandleDifference) / 100) * (_data[i - 1].Close - _data[i - 1].Open) < _data[i - 2].Close - _data[i - 2].Open && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && ((100 + variables._minCandleDifference) / 100) * (_data[i - 0].Close - _data[i - 0].Open) < _data[i - 1].Close - _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBeltHold(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].High < _data[i - 0].Low)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].High && _data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackClosingMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High > _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackOpeningMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close > _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBreakaway(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 4].High < _data[i - 3].Low && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 3].Close < _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < variables._minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._minCandleSize)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 4].High && _data[i - 0].Close < _data[i - 3].Low)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDeliberation(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Open < _data[i - 1].Open && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close <= _data[i - 0].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < variables._minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDarkCloudCover(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && _data[i - 0].Close < _data[i - 1].Open + ((_data[i - 1].Close - _data[i - 1].Open) / 2))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDojiStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < variables._maxDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDownsideGap3Methods(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 1].Open > _data[i - 0].Open && _data[i - 2].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDownsideTasukiGap(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 1].Open > _data[i - 0].Open && _data[i - 2].Low > _data[i - 0].Close && _data[i - 1].High < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDragonflyDoji(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > variables._minCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEngulfing(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEveningDojiStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < variables._maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 2].Open && _data[i - 0].Close < _data[i - 2].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEveningStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 2].Open && _data[i - 0].Close < _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishFalling3Methods(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Open && _data[i - 4].Close < _data[i - 3].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Close) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Open < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Close < _data[i - 4].Open && Math.Max(_data[i - 2].Close, _data[i - 2].Open) < _data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 4].Close && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize))
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishGravestoneDoji(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishHarami(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishIdentical3Crows(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < -1 * variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close == _data[i - 1].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < -1 * variables._minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close == _data[i - 0].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < -1 * variables._minCandleSize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishHaramiCross(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishInNeck(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close < variables._inBarMaxChange)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishKicking(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Open - _data[i - 1].Low) / _data[i - 1].Open < variables._maxShadowSize && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < variables._maxShadowSize && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Open > _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < variables._maxShadowSize && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShadowSize && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishLongBlackCandelstick(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishMeetingLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && (100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > variables._minCandleSize) && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < variables._maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishOnNeck(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Low) / _data[i - 0].Close) < variables._maxPriceDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishSeparatingLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < variables._maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishShootingStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && 3 * Math.Abs(_data[i - 0].Open - _data[i - 0].Close) < _data[i - 0].High - _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxCandleBodySize && 100 * (Math.Min(_data[i - 0].Open, _data[i - 0].Close) - _data[i - 0].Low) / Math.Min(_data[i - 0].Open, _data[i - 0].Close) < variables._maxCandleShadowSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishSideBySideWhiteLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].High < _data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 1].Open) / _data[i - 0].Open) < variables._maxDifference && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 0].Close) < variables._maxDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishThrusting(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishTriStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < variables._maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < variables._maxDojiBodySize && _data[i - 2].High < _data[i - 1].Low && _data[i - 0].High < _data[i - 1].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if ((Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishTweezerTop(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open >= _data[i - 0].Close && _data[i - 0].High == _data[i - 1].High && (Math.Abs(100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < variables._maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishUpsideGap2Crows(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && _data[i - 1].Open > _data[i - 1].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close < _data[i - 1].Close && _data[i - 0].Close > _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3InsideUp(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShortCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close > _data[i - 1].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3OutsideUp(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < variables._maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close > _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3StarsintheSouth(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Open) / _data[i - 2].Open < variables._maxShadowChange && 100 * (_data[i - 2].Close - _data[i - 2].Low) / _data[i - 2].Low > variables._minCandleSize && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShadowChange && _data[i - 1].Low > _data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].High && _data[i - 0].Close > _data[i - 1].Low && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShadowChange && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Low < variables._maxShadowChange)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3WhiteSoldiers(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < variables._maxCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < variables._maxCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < variables._maxCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3LineStrike(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open < _data[i - 3].Close && 100 * (_data[i - 3].High - _data[i - 3].Close) / _data[i - 3].Close < variables._maxCloseHighChange && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open < _data[i - 2].Close && _data[i - 3].Close > _data[i - 2].Open && _data[i - 2].Open > _data[i - 3].Open && _data[i - 2].Close > _data[i - 3].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < variables._maxCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < variables._maxCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishBeltHold(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Low > _data[i - 0].High)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].Low && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < variables._maxCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishBreakaway(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High < _data[i - 4].Low && (-100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < variables._minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 3].Close > _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < variables._minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._minCandleSize))
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 4].Low && _data[i - 0].Close > _data[i - 3].High)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishConcealingBabySwallow(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High == _data[i - 3].Open && _data[i - 3].Low == _data[i - 3].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].High == _data[i - 2].Open && _data[i - 2].Low == _data[i - 2].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].High > _data[i - 2].Close)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open >= _data[i - 1].High && _data[i - 0].Close < _data[i - 1].Low)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishDojiStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < variables._maxDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishDragonflyDoji(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > variables._minCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishEngulfing(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishGravestoneDoji(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHarami(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHaramiCross(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize && _data[i - 0].Close < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHomingPigeon(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShortCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishInvertedHammer(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close < variables._maxShadowSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && _data[i - 0].High - _data[i - 0].Low > 3 * Math.Abs(_data[i - 0].Close - _data[i - 0].Open) && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < variables._maxShadowSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < variables._maxCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishKicking(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < variables._maxShadowSize && -100 * (_data[i - 1].Low - _data[i - 1].Close) / _data[i - 1].Close < variables._maxShadowSize && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < variables._maxShadowSize && -100 * (_data[i - 0].Low - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShadowSize && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishLadderBottom(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if ((_data[i - 4].Open > _data[i - 4].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if ((_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Close && _data[i - 4].Close <= _data[i - 3].Open && _data[i - 3].Open <= _data[i - 4].Open && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > variables._minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize))
                        {
                            // Check whether the fourth candlestick matches. 
                            if ((_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].High - _data[i - 1].Open > 2 * (_data[i - 1].Open - _data[i - 1].Close)))
                            {
                                // Check whether the fifth candlestick matches. 
                                if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open))
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishLongWhiteCandlestick(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMatHold(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].Close < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 3].Open && _data[i - 2].Open > _data[i - 4].Open) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 4].Close && _data[i - 0].Close > _data[i - 3].Open)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMatchingLow(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close == _data[i - 1].Close)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMeetingLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > variables._minCandleSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < variables._maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMorningDojiStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < variables._maxDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < variables._maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches.  
                        if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMorningStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < variables._maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches.  
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishPiercingLine(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishRising3Methods(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].High > _data[i - 3].Open && _data[i - 4].Low < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Open && _data[i - 4].High > _data[i - 2].Open && _data[i - 4].High > _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Open)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < Math.Min(_data[i - 2].Open, _data[i - 2].Close) && _data[i - 4].High > _data[i - 1].Open && _data[i - 4].Low < _data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close > _data[i - 4].Close)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishSeparatingLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < variables._maxOpenDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishSideBySideWhiteLines(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < variables._maxDifference && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < variables._maxDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishStickSandwich(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Close)
                    {
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 2].Close == _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishTriStar(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < variables._maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < variables._maxDojiBodySize && _data[i - 1].High < _data[i - 2].Low && _data[i - 1].High < _data[i - 0].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxDojiBodySize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishTweezerBottom(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (100 * Math.Abs((_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open) > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < variables._maxShortCandleSize && _data[i - 0].Low == _data[i - 1].Low)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUnique3RiverBottom(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open == _data[i - 1].High && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches.  
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < variables._maxShortCandleSize && _data[i - 1].Close > _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUpsideGap3Methods(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > variables._minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > variables._minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Open < _data[i - 0].Open && _data[i - 2].Close > _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUpsideTasukiGap(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Open < _data[i - 0].Open && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 0].Close && _data[i - 2].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteClosingMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open != _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteOpeningMarubozu(List<OhlcvObject> data, VariablesModel variables)
        {
            var _data = data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume, Symbol = x.Symbol }).ToList();

            if (data[0].Symbol != _data[0].Symbol) return new List<OhlcvObject>();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > variables._minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        private List<OhlcvObject> GetSignals(string patternMethodName, List<OhlcvObject> data, VariablesModel variables)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(patternMethodName);

            List<OhlcvObject> result = (List<OhlcvObject>)theMethod.Invoke(this, new object[] { data, variables });
            return result;
        }

        private int GetSignalsCount(string patternMethodName, List<OhlcvObject> data, VariablesModel variables)
        {
            List<OhlcvObject> result = GetSignals(patternMethodName, data, variables);

            return result.Where(x => x.Signal == true).Count();
        }

        public int GetBullishSignalsCount(List<OhlcvObject> data)
        {
            var variables = GetVariables(data);
            var bullish = GetAllMethods(data).Where(x => x.StartsWith("Bullish")).ToList();

            List<int> bullishQty = new List<int>();

            foreach (var methodName in bullish)
            {
                bullishQty.Add(GetSignalsCount(methodName, data, variables));
            }

            return bullishQty.Sum(x => x);
        }

        public int GetBearishSignalsCount(List<OhlcvObject> data)
        {
            var variables = GetVariables(data);
            var bearish = GetAllMethods(data).Where(x => x.StartsWith("Bearish")).ToList();

            List<int> bearishhQty = new List<int>();

            foreach (var methodName in bearish)
            {
                bearishhQty.Add(GetSignalsCount(methodName, data, variables));
            }

            return bearishhQty.Sum(x => x);
        }

        private List<string> GetAllMethods(List<OhlcvObject> data)
        {
            List<string> methods = new List<string>();

            foreach (MethodInfo item in typeof(Patterns).GetMethods())
            {
                methods.Add(item.Name);
            }

            return methods;
        }
    }
}
