﻿using System;

namespace SE_mining_base.Transactios.Models
{
    public class BalanceRow
    {
        public DateTime Date => _date;
        private DateTime _date;
        public double TransactionCost => _transactionCost;
        private double _transactionCost;
        public double TransactionMargin => _transactionMargin;
        private double _transactionMargin;
        public double Total => _total;
        private double _total;
        public Guid RequestId => _requestId;
        private Guid _requestId;

        private BalanceRow()
        {
        }

        public class Builder
        {
            private DateTime _date;
            private double _transactionCost;
            private double _transactionMargin;
            private double _total;
            private Guid _requestId;

            public Builder WithRequestId(Guid value)
            {
                _requestId = value;
                return this;
            }

            public Builder WithDate(DateTime value)
            {
                _date = value;
                return this;
            }

            public Builder TransactionCost(double value)
            {
                _transactionCost = value;
                return this;
            }

            public Builder TransactionMargin(double value)
            {
                _transactionMargin = value;
                return this;
            }

            public Builder Total(double value)
            {
                _total = value;
                return this;
            }

            public BalanceRow Build()
            {
                return new BalanceRow()
                {
                    _date = _date,
                    _transactionCost = _transactionCost,
                    _transactionMargin = _transactionMargin,
                    _total = _total,
                    _requestId = _requestId
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}," +
                   $" {nameof(TransactionCost)}: {TransactionCost}," +
                   $" {nameof(TransactionMargin)}: {TransactionMargin}, " +
                   $" {nameof(RequestId)}: {RequestId}, " +
                   $"{nameof(Total)}: {Total}";
        }
    }
}
