#pragma once

#include "Common.ice"
#include <Ice/Identity.ice>

module PortServerIce{

	enum PSIceCallbackReportType {
		ReportConfirmed,	//申报确认
		ReportError,		//申报错误
		ReportScraped 	//申报废单
	};

	enum PSIceCallbackCancelType 
	{
		CancelSuccess,	//撤单成功
		CancelFail,		//撤单失败
		CancelScraped		//撤单废单
	};

	//委托成交回报
	struct PSIceCallbackTransaction
	{
		string OrderNo;		//委托编号
		string TransNo;		//成交编号
		PSIceMarket Market;		//交易所
		string StockAccount;	//股东编号
		int Side;			//买卖标志
		string Code;		//股票代码
		long OrderPrice;	//委托价格
		int OrderVolume;	//委托数量
		int FilledVolume;	//本次成交数量
		long FilledPrice;	//本次成交价格
		long FilledAmount;	//本次成交金额
		long TotalAmount;	//总成交金额，本笔成交处理后
		int TotalVolume;	//总成交数量，本笔成交处理后
		string Message;		//成交摘要
	};

	//委托申报回报
	struct PSIceCallbackReport
	{
		string OrderNo;		//委托编号
		PSIceMarket Market;	//交易所
		string StockAccount; //股东编号
		int Side;			//买卖标志
		string Code;		//股票代码
		PSIceCallbackReportType Type;	//消息类型
		int ReportCode;		//申报结果代码
		string ReportMessage;	//申报结果消息
	};

	//委托撤单确认回报
	struct PSIceCallbackCancel
	{
		string OrderNo;		//委托编号
		PSIceMarket Market;	//交易所
		string StockAccount;//股东编号
		string Code;		//股票代码
		int OrderVolume;	//委托数量
		int CancelVolume;	//撤单数量
		int FilledVolume;	//成交数量
		PSIceCallbackCancelType Type;	//撤销类型
		int CancelCode;		//撤单结果代码
		string CancelMessage; //撤单结果消息
	};


	//交易回调发送端
	interface TradeCallbackSender
	{
		void Heartbeat();

		void Start(string name, Ice::Identity ident);

		void Stop();
	};

	//交易回调接收
	interface TradeCallbackReceiver
	{
		void Notify();

		void CallbackTransaction(PSIceCallbackTransaction transaction);

		void CallbackReport(PSIceCallbackReport report);

		void CallbackCancel(PSIceCallbackCancel cancel);
	};
};