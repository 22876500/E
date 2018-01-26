#pragma once

module PortServerIce{
	exception PortServerIceException {
	};

	enum PSIceMarket {
		SH,		//沪市A
		SZ,		//深市A
	};

	enum PSIceFundType{
		Normal,		//普通账户
		Credit,		//信用账户
	};

	//委托状态
	enum PSIceOrderState {
		Undeclared,		//未申报
		Declaring,		//正在申报
		Declared,		//已申报
		PendingCancel,		//已报待撤
		PFPendingCancel,	//部成待撤
		PartCanceled,   //部撤
		Canceled,		//已撤
		PartFilled,     //部分成交
		Filled,			//已成交
		Scraped,		//废单
		Error,			//错误
		Other,			//其他未知
	};

	//委托方式
	enum PSIceOrderType {
		Buy,	//普通买入
		Sell,	//普通卖出
		CreditBuy,	//担保品买入
		CreditSell,	//担保品卖出
		CashBuy,	//融资买入
		CashSell,   //卖券还款
		ShareSell,  //融券卖出
		ShareBuy,   //买券还券
		ShareRefund,	//现券还券
		CashRefund,		//现金还款
		None,		//未知
	};

	//账户基础信息
	struct PSIceAccount {
		int Id;					//客户ID
		int ClientId;			//客户编号ID  金证：fundid资金账号，恒生：client_id客户编号
		string ClientName;		//客户编号    顶点：FID_KHH客户号
		string Password;		//交易密码
		string ComPassword;		//通信密码
		PSIceFundType FundType;	//账户类型
		string BrokerName;		//所属券商
		string DepartmentNo;	//券商营业部代码
		string FundName;		//资金账号
		string SHStockAccount;	//沪市股东账号
		string SZStockAccount;	//深市股东账号
		string Node;			//节点信息,顶点ip，mac，硬盘序号
	};

	struct PSIceErrorCode
	{
		long Code;
		string Message;
	};

	
};