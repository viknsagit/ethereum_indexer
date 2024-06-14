using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Blockchain_Indexer.Blockchain.TokensFunctions;

internal class ERC20_Functions
{
    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner")] public string Owner { get; set; }
    }

    [Function("name", "string")]
    public class NameFunction : FunctionMessage
    {
    }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunction : FunctionMessage
    {
    }

    [Function("decimals", "uint8")]
    public class DecimalsFunction : FunctionMessage
    {
    }

    [Function("symbol", "string")]
    public class SymbolFunction : FunctionMessage
    {
    }

    [Event("Transfer")]
    public class TransferEvent : IEventDTO
    {
        [Parameter("address", "from", 1, true)]
        public string From { get; set; }

        [Parameter("address", "to", 2, true)] public string To { get; set; }

        [Parameter("uint256", "value", 3, false)]
        public BigInteger Value { get; set; }
    }
}