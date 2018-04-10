#pragma once

#include "Common.ice"

module PortServerIce{
	
	//资金账户
	struct PSIceFundAccount
	{
		int ClientId;		//客户编号ID  金证：fundid资金账号，恒生：client_id客户编号
		string ClientName;	//客户编号    顶点：FID_KHH客户号
		int FundId;			//资金账号ID  金证：custid客户代码
		string FundName;	//资金账号    顶点：FID_ZJZH资金账号 恒生：fund_account资金账号
		PSIceFundType FundType;	//账户类型
	};

	sequence<PSIceFundAccount> PSIceFundAccounts;

	//股东信息
	struct PSIceStockAccount
	{
		int FundId;			//资金账号ID
		string FundName;		//资金账号
		string StockAccount;	//股东编号
		PSIceMarket Market;		//交易所
	};

	//股东列表
	sequence<PSIceStockAccount> PSIceStockAccounts;

	//资金信息
	["clr:class"]struct PSIceFundSummary
	{
		long EnableMargin;		//可用保证金
		long UsedMargin;		//已用保证金
		long EnableFund;		//可用资金（买担保品资金）
		long CashBuyFund;		//融资买入可用资金
		long ShareBuyFund;		//买券还券可用资金
		long CashRefundFund;	//现金还款可用资金

		long EnableCashCredit;		//融资可用额度
		long UsedCashCredit;		//融资已用额度
		long UsedCashMargin;		//融资已用保证金
		long CashDebt;				//融资合约金额（融资负债）

		long EnableShareCredit;		//融券可用额度
		long UsedShareCredit;		//融券已用额度
		long UsedShareMargin;		//融券已用保证金
		long ShareDebt;				//融券合约金额（融券负债）

		long NetAsset;				//净资产
	};

	//持仓信息
	struct PSIcePosition
	{
		string Index;			//定位串
		PSIceMarket Market;		//交易所
		string StockAccount;	//股东编号
		string Code;		//股票代码
		int CurrentVolume;	//当前数量
		int EnableVolume;	//可用(卖)数量
		int BuyVolume;		//今日买入数量
		int SellVolume;		//今日卖出数量
	};

	sequence<PSIcePosition> PSIcePositionList;

	//可融券信息
	struct PSIceSharePosition
	{
		string Index;				//定位串
		PSIceMarket Market;			//交易所
		string StockAccount;		//股东编号
		string Code;				//股票代码
		int ShareVolume;			//可融券数量
	};

	sequence<PSIceSharePosition> PSIceSharePositionList;

	struct PSIceVolumeSearch
	{
		string Index;			//定位串
		PSIceMarket Market;		//交易所
		string Code;			//股票代码
		PSIceOrderType OrderType;	//委托类型
	};

	//担保品信息
	struct PSIceAssureInfo
	{
		string Index;		//定位串
		PSIceMarket Market;	//交易所
		string Code;		//股票代码
		double AssureRatio;		//担保品折算比例
		double FloatRatio;		//浮动比例
		double Price;		//市值价
	};

	sequence<PSIceAssureInfo> PSIceAssureInfoList;

	//标的信息
	struct PSIceObjectInfo
	{
		string Index;		//定位串
		PSIceMarket Market;	//交易所
		string Code;		//股票代码
		double CashRatio;	//融资担保比例
		double CashFloatRatio;	//融资浮动比例
		double ShareRatio;	//融券担保比例
		double ShareFloatRatio;	//融券浮动比例
		double Price;			//市值价
	};

	sequence<PSIceObjectInfo> PSIceObjectInfoList;

	//批量委托查询
	struct PSIceOrderSearch
	{
		string Index;			//索引号
		string OrderNo;		//委托编号
		string Code;		//证券代码
		int Type;			//查询类型  1查询全部委托 2查询可撤委托
	};

	//委托状态
	struct PSIceOrderStatus
	{
		string Index;	//定位串
		string OrderNo;	//委托编号
		PSIceMarket Market;	//交易所
		string StockAccount;		//股东编号
		PSIceOrderType OrderType;	//委托类别
		int Side;			//买卖标志  1买入 2卖出
		string Code;		//股票代码
		long OrderPrice;	//委托价格
		int OrderVolume;	//委托数量
		int OrderTime;		//委托时间
		long FilledPrice;	//成交价格
		int FilledVolume;	//成交数量
		long FilledAmount;	//成交金额
		int FilledCount;	//成交次数
		bool IsCancel;		//是否撤销
		int CancelVolume;	//撤单数量
		PSIceOrderState OrderState;	//委托状态
		string Message;		//委托消息
	};

	sequence<PSIceOrderStatus> PSIceOrderStatusList;

	//成交状态
	struct PSIceTransStatus
	{
		string Index;		//定位串
		string OrderNo;		//委托编号
		string TransNo;		//成交编号
		PSIceMarket Market;		//交易所
		string StockAccount;		//股东编号
		PSIceOrderType OrderType;	//委托类别
		int Side;			//买卖标志  1买入 2卖出
		string Code;		//股票代码
		long FilledPrice;	//成交价格
		int FilledVolume;	//成交数量
		long FilledAmount;	//成交金额
		int FilledTime;		//成交时间
		bool IsCancel;		//是否撤销
		int CancelVolume;	//撤单数量
	};

	sequence<PSIceTransStatus> PSIceTransStatusList;

	//合约查询
	struct PSIceOrderContractSearch
	{
		string Index;		//定位串
		int ContractType;	//查询合约类型 0融券 1融资
		int ContractState;	//合约状态 0全部 1当日了结 2未了结
		string Code;		//证券代码
		int StartDate;	    //开始日期，0表示今天，整数20150430
		int EndDate;		//结束日期
	};

	//合约信息
	struct PSIceOrderContract
	{
		string Index;		//定位串
		string OrderNo;		//委托编号
		string ContractNo;	//合约编号
		int ContractType;	//合约类型 0融券 1融资
		string Code;		//股票代码
		int ShareVolume;	//融券数量
		int DebtVolume;		//未还数量
		int RefundVolume;	//已还数量
		long RefundFund;	//买券还券可用资金
		long DebtAmount;	//负债金额
		long RefundAmount;	//已还金额
		int OpenDate;	    //开仓日期
	};

	sequence<PSIceOrderContract> PSIceOrderContractList;


	//委托详情
	struct PSIceOrderDetail
	{
		string Index;		//定位串
		string OrderNo;		//委托编号
		int OrderDate;		//委托日期
		int OrderTime;		//委托时间
		int ReportTime;		//申报时间
		PSIceMarket Market;	//交易所
		string StockAccount;		//股东编号
		PSIceOrderType OrderType;	//委托类别
		int Side;			//买卖标志  1买入 2卖出
		string Code;		//股票代码
		long OrderPrice;	//委托价格
		int OrderVolume;	//委托数量
		long FilledPrice;	//成交价格
		int FilledVolume;	//成交数量
		long FilledAmount;	//成交金额
		int FilledTime;		//成交时间
		bool IsCancel;		//是否撤销
		int CancelVolume;	//撤单数量
		PSIceOrderState OrderState;	//委托状态
	};

	//委托详情列表
	sequence<PSIceOrderDetail> PSIceOrderDetailList;

	//成交详情
	struct PSIceTransDetail
	{
		string Index;			//定位串
		string OrderNo;			//委托编号
		string TransNo;			//成交编号
		PSIceMarket Market;		//交易所
		string StockAccount;		//股东编号
		PSIceOrderType OrderType;	//委托类别
		int Side;				//买卖标志  1买入 2卖出
		string Code;			//股票代码
		long FilledPrice;		//成交价格
		int FilledVolume;		//成交数量
		long FilledAmount;		//成交金额
		bool IsCancel;			//是否撤销
		int CancelVolume;		//撤单数量
		int FilledDate;			//成交日期
		int FilledTime;			//成交时间
	};

	//成交详情列表
	sequence<PSIceTransDetail> PSIceTransDetailList;

	//交割单详情
	struct PSIceTradeDetail
	{
		string Index;		//定位串
		int TradeDate;		//清算日期
		string OrderNo;		//委托编号
		PSIceMarket Market;	//交易所
		string StockAccount;		//股东编号
		PSIceOrderType OrderType;	//委托类别
		string Code;		//股票代码
		int MatchTimes;		//成交笔数
		int MatchVolume;	//成交股数
		long MatchPrice;	//成交价格
		long MatchAmount;	//成交金额
		long FundEffect;	//清算金额
		long FeeYHS;		//印花税
		long FeeSXF;		//手续费
		long FeeGHF;		//过户费
		long FeeYJ;			//佣金
		long FeeOther;		//其他费用
	};

	//交割单详情列表
	sequence<PSIceTradeDetail> PSIceTradeDetailList;

	//查询类接口，一般都有查询次数限制
	interface QueryServant
	{
		//连接接口
		bool Connect(PSIceAccount account);

		//断开接口
		bool Disconnect(PSIceAccount account);

		//客户登陆
		int Login(PSIceAccount account, out PSIceErrorCode error);

		//客户登出
		int Logout(PSIceAccount account, out PSIceErrorCode error);

		//订阅所有消息
		bool SubscribAll(PSIceAccount account);

		//取消订阅所有消息
		bool UnsubscribAll(PSIceAccount account);

		//获取客户信息
		int GetFundAccountList(PSIceAccount account, out PSIceFundAccounts fundAccounts, out PSIceErrorCode error);

		//获取股东列表
		int GetStockAccountList(PSIceAccount account, out PSIceStockAccounts stockAccounts, out PSIceErrorCode error);

		//查询持仓列表
		int GetPositionList(PSIceAccount account, out PSIcePositionList positions, out PSIceErrorCode error);

		//获取资金信息
		int GetFundSummary(PSIceAccount account, out PSIceFundSummary fund, out PSIceErrorCode error);

		//查询融券余额
		int GetSharePositionList(PSIceAccount account, out PSIceSharePositionList positions, out PSIceErrorCode error);

		//查询公共池余券
		int GetBrokerPositionList(PSIceAccount account, string index, out PSIceSharePositionList positions, out PSIceErrorCode error);

		//查询可买卖数量
		int GetEnableVolume(PSIceAccount account, PSIceVolumeSearch search, out int volume, out PSIceErrorCode error);

		//查询担保品信息
		int GetAssureInfoList(PSIceAccount account, string index, out PSIceAssureInfoList infolist, out PSIceErrorCode error);

		//查询标的证券信息
		int GetObjectInfoList(PSIceAccount account, string index, out PSIceObjectInfoList infolist, out PSIceErrorCode error);

		//查询信用合约列表
		int GetOrderContractList(PSIceAccount account, PSIceOrderContractSearch search, out PSIceOrderContractList contracts, out PSIceErrorCode error);

		//查询当日委托状态
		int GetTodayOrderStatusList(PSIceAccount account, PSIceOrderSearch search, out PSIceOrderStatusList statuslist, out PSIceErrorCode error);

		//查询当日成交状态
		int GetTodayTransStatusList(PSIceAccount account, PSIceOrderSearch search, out PSIceTransStatusList statuslist, out PSIceErrorCode error);
		
		//查询委托列表, 0表示今天，整数20150430
		int GetOrderDetailList(int date, PSIceAccount account, string index, out PSIceOrderDetailList orderlist, out PSIceErrorCode error);

		//查询成交列表
		int GetTransDetailList(int date, PSIceAccount account, string index, out PSIceTransDetailList Translist, out PSIceErrorCode error);

		//查询交割单列表
		int GetTradeDetailList(int date, PSIceAccount account, string index, out PSIceTradeDetailList tradelist, out PSIceErrorCode error);

		
	};
};