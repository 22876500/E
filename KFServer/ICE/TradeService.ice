#pragma once

#include "Common.ice"

module PortServerIce{

	struct PSIceOrder
	{
		int BatchId;		//委托批号
		string ClOrderId;	//客户端OrderId
		PSIceMarket Market;	//股票市场
		PSIceOrderType OrderType;  //委托类型
		string Code;		//股票代码
		long Price;			//委托价格
		int Volume;			//委托数量
		string Node;		//节点信息
	};

	struct PSIceOrderOut
	{
		string ClOrderId;	//客户端OrderId
		string OrderNo;	//委托编号
		int BatchId;		//委托批号
	};

	struct PSIceOrderCancel
	{
		PSIceOrderType OrderType;  //委托类型
		string ClOrderId;	//客户端OrderId
		int BatchId;		//委托批号
		string OrderNo;		//委托编号
		PSIceMarket Market; //交易所
		string Code;		//股票代码
		int CancelVolume;	//撤单数量
		string Node;		//节点信息
	};

	struct PSIceOrderCancelOut
	{
		string ClOrderId;	//客户端OrderId
		string OrderNo;		//委托编号
	};

	["clr:class"]struct PSIceCashRefund
	{
		long FundEffect;	//偿还金额
		string Node;		//节点信息
	};

	["clr:class"]struct PSIceCashRefundOut
	{
		long Effect;			//实际还款额
	};

	struct PSIceShareRefund
	{
		string ClOrderId;	//客户端OrderId
		int BatchId;		//委托批号
		string ContractNo;	//合约编号
		PSIceMarket Market;	//股票市场
		string Code;		//股票代码
		int Volume;			//还券数量
		string Node;		//节点信息
	};

	struct PSIceShareRefundOut
	{
		string ClOrderId;	//客户端OrderId
		string OrderNo;		//委托编号
		int BatchId;		//委托批号
	};

	//交易类接口，无查询次数限制
	interface TradeServant
	{
		//放单
		int SendOrder(PSIceAccount account, PSIceOrder order, out PSIceOrderOut orderout, out PSIceErrorCode error);

		//撤单
		int CancelOrder(PSIceAccount account, PSIceOrderCancel cancel, out PSIceOrderCancelOut cancelout, out PSIceErrorCode error);

		//现券还券
		int RefundShare(PSIceAccount account, PSIceShareRefund share, out PSIceShareRefundOut refundout, out PSIceErrorCode error);

		//现金还款,返回实际还款额
		int RefundCash(PSIceAccount account, PSIceCashRefund cash, out PSIceCashRefundOut refundout, out PSIceErrorCode error);

	};
};