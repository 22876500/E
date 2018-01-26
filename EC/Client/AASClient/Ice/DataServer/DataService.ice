#include "Common.ice"

module DataServerIce {

	sequence<string> DSIceCodes;	//股票代码列表
	
	struct DSIceStockCode {
		string Wind;
		string Code;
		string Market;
		string Name;
		string Pinyin;
		int Type;
	};

	sequence<DSIceStockCode> DSIceStockCodes;
	
	interface DataServant
	{
		/**
		* 订阅股票行情
		*/
		int SubscribeCodes(string username, DSIceCodes codelist);

		/**
		* 取消订阅股票行情
		*/
		int UnsubscribeCodes(string username, DSIceCodes codelist);

		/**
		* 刷新股票订阅列表，长时间未刷新的股票信息将自动取消订阅
		*/
		void FlushCodes(string username, DSIceCodes codelist);

		/**
		* 获取股票代码定义列表
		*/
		int GetStockCodes(out DSIceStockCodes codes);

		/**
		* 获取给定的股票代码
		*/
		string GetVipCodes();

		/**
		* 获取股票代码定义列表
		*/
		string GetStockCodesInfo();

		/**
		* 设置订阅模式
		*/
		bool SetSubType(string username,int subType);

		/**
		* 获取订阅模式
		*/
		int GetSubType();
		
		/*
		*获取pub数据的类型
		*/
		int GetPubType();

	};
};