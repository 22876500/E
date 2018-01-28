#pragma once

#include "Common.ice"

module PortServerIce{
	
	//�ʽ��˻�
	struct PSIceFundAccount
	{
		int ClientId;		//�ͻ����ID  ��֤��fundid�ʽ��˺ţ�������client_id�ͻ����
		string ClientName;	//�ͻ����    ���㣺FID_KHH�ͻ���
		int FundId;			//�ʽ��˺�ID  ��֤��custid�ͻ�����
		string FundName;	//�ʽ��˺�    ���㣺FID_ZJZH�ʽ��˺� ������fund_account�ʽ��˺�
		PSIceFundType FundType;	//�˻�����
	};

	sequence<PSIceFundAccount> PSIceFundAccounts;

	//�ɶ���Ϣ
	struct PSIceStockAccount
	{
		int FundId;			//�ʽ��˺�ID
		string FundName;		//�ʽ��˺�
		string StockAccount;	//�ɶ����
		PSIceMarket Market;		//������
	};

	//�ɶ��б�
	sequence<PSIceStockAccount> PSIceStockAccounts;

	//�ʽ���Ϣ
	["clr:class"]struct PSIceFundSummary
	{
		long EnableMargin;		//���ñ�֤��
		long UsedMargin;		//���ñ�֤��
		long EnableFund;		//�����ʽ��򵣱�Ʒ�ʽ�
		long CashBuyFund;		//������������ʽ�
		long ShareBuyFund;		//��ȯ��ȯ�����ʽ�
		long CashRefundFund;	//�ֽ𻹿�����ʽ�

		long EnableCashCredit;		//���ʿ��ö��
		long UsedCashCredit;		//�������ö��
		long UsedCashMargin;		//�������ñ�֤��
		long CashDebt;				//���ʺ�Լ�����ʸ�ծ��

		long EnableShareCredit;		//��ȯ���ö��
		long UsedShareCredit;		//��ȯ���ö��
		long UsedShareMargin;		//��ȯ���ñ�֤��
		long ShareDebt;				//��ȯ��Լ����ȯ��ծ��

		long NetAsset;				//���ʲ�
	};

	//�ֲ���Ϣ
	struct PSIcePosition
	{
		string Index;			//��λ��
		PSIceMarket Market;		//������
		string StockAccount;	//�ɶ����
		string Code;		//��Ʊ����
		int CurrentVolume;	//��ǰ����
		int EnableVolume;	//����(��)����
		int BuyVolume;		//������������
		int SellVolume;		//������������
	};

	sequence<PSIcePosition> PSIcePositionList;

	//����ȯ��Ϣ
	struct PSIceSharePosition
	{
		string Index;				//��λ��
		PSIceMarket Market;			//������
		string StockAccount;		//�ɶ����
		string Code;				//��Ʊ����
		int ShareVolume;			//����ȯ����
	};

	sequence<PSIceSharePosition> PSIceSharePositionList;

	struct PSIceVolumeSearch
	{
		string Index;			//��λ��
		PSIceMarket Market;		//������
		string Code;			//��Ʊ����
		PSIceOrderType OrderType;	//ί������
	};

	//����Ʒ��Ϣ
	struct PSIceAssureInfo
	{
		string Index;		//��λ��
		PSIceMarket Market;	//������
		string Code;		//��Ʊ����
		double AssureRatio;		//����Ʒ�������
		double FloatRatio;		//��������
		double Price;		//��ֵ��
	};

	sequence<PSIceAssureInfo> PSIceAssureInfoList;

	//�����Ϣ
	struct PSIceObjectInfo
	{
		string Index;		//��λ��
		PSIceMarket Market;	//������
		string Code;		//��Ʊ����
		double CashRatio;	//���ʵ�������
		double CashFloatRatio;	//���ʸ�������
		double ShareRatio;	//��ȯ��������
		double ShareFloatRatio;	//��ȯ��������
		double Price;			//��ֵ��
	};

	sequence<PSIceObjectInfo> PSIceObjectInfoList;

	//����ί�в�ѯ
	struct PSIceOrderSearch
	{
		string Index;			//������
		string OrderNo;		//ί�б��
		string Code;		//֤ȯ����
		int Type;			//��ѯ����  1��ѯȫ��ί�� 2��ѯ�ɳ�ί��
	};

	//ί��״̬
	struct PSIceOrderStatus
	{
		string Index;	//��λ��
		string OrderNo;	//ί�б��
		PSIceMarket Market;	//������
		string StockAccount;		//�ɶ����
		PSIceOrderType OrderType;	//ί�����
		int Side;			//������־  1���� 2����
		string Code;		//��Ʊ����
		long OrderPrice;	//ί�м۸�
		int OrderVolume;	//ί������
		int OrderTime;		//ί��ʱ��
		long FilledPrice;	//�ɽ��۸�
		int FilledVolume;	//�ɽ�����
		long FilledAmount;	//�ɽ����
		int FilledCount;	//�ɽ�����
		bool IsCancel;		//�Ƿ���
		int CancelVolume;	//��������
		PSIceOrderState OrderState;	//ί��״̬
		string Message;		//ί����Ϣ
	};

	sequence<PSIceOrderStatus> PSIceOrderStatusList;

	//�ɽ�״̬
	struct PSIceTransStatus
	{
		string Index;		//��λ��
		string OrderNo;		//ί�б��
		string TransNo;		//�ɽ����
		PSIceMarket Market;		//������
		string StockAccount;		//�ɶ����
		PSIceOrderType OrderType;	//ί�����
		int Side;			//������־  1���� 2����
		string Code;		//��Ʊ����
		long FilledPrice;	//�ɽ��۸�
		int FilledVolume;	//�ɽ�����
		long FilledAmount;	//�ɽ����
		int FilledTime;		//�ɽ�ʱ��
		bool IsCancel;		//�Ƿ���
		int CancelVolume;	//��������
	};

	sequence<PSIceTransStatus> PSIceTransStatusList;

	//��Լ��ѯ
	struct PSIceOrderContractSearch
	{
		string Index;		//��λ��
		int ContractType;	//��ѯ��Լ���� 0��ȯ 1����
		int ContractState;	//��Լ״̬ 0ȫ�� 1�����˽� 2δ�˽�
		string Code;		//֤ȯ����
		int StartDate;	    //��ʼ���ڣ�0��ʾ���죬����20150430
		int EndDate;		//��������
	};

	//��Լ��Ϣ
	struct PSIceOrderContract
	{
		string Index;		//��λ��
		string OrderNo;		//ί�б��
		string ContractNo;	//��Լ���
		int ContractType;	//��Լ���� 0��ȯ 1����
		string Code;		//��Ʊ����
		int ShareVolume;	//��ȯ����
		int DebtVolume;		//δ������
		int RefundVolume;	//�ѻ�����
		long RefundFund;	//��ȯ��ȯ�����ʽ�
		long DebtAmount;	//��ծ���
		long RefundAmount;	//�ѻ����
		int OpenDate;	    //��������
	};

	sequence<PSIceOrderContract> PSIceOrderContractList;


	//ί������
	struct PSIceOrderDetail
	{
		string Index;		//��λ��
		string OrderNo;		//ί�б��
		int OrderDate;		//ί������
		int OrderTime;		//ί��ʱ��
		int ReportTime;		//�걨ʱ��
		PSIceMarket Market;	//������
		string StockAccount;		//�ɶ����
		PSIceOrderType OrderType;	//ί�����
		int Side;			//������־  1���� 2����
		string Code;		//��Ʊ����
		long OrderPrice;	//ί�м۸�
		int OrderVolume;	//ί������
		long FilledPrice;	//�ɽ��۸�
		int FilledVolume;	//�ɽ�����
		long FilledAmount;	//�ɽ����
		int FilledTime;		//�ɽ�ʱ��
		bool IsCancel;		//�Ƿ���
		int CancelVolume;	//��������
		PSIceOrderState OrderState;	//ί��״̬
	};

	//ί�������б�
	sequence<PSIceOrderDetail> PSIceOrderDetailList;

	//�ɽ�����
	struct PSIceTransDetail
	{
		string Index;			//��λ��
		string OrderNo;			//ί�б��
		string TransNo;			//�ɽ����
		PSIceMarket Market;		//������
		string StockAccount;		//�ɶ����
		PSIceOrderType OrderType;	//ί�����
		int Side;				//������־  1���� 2����
		string Code;			//��Ʊ����
		long FilledPrice;		//�ɽ��۸�
		int FilledVolume;		//�ɽ�����
		long FilledAmount;		//�ɽ����
		bool IsCancel;			//�Ƿ���
		int CancelVolume;		//��������
		int FilledDate;			//�ɽ�����
		int FilledTime;			//�ɽ�ʱ��
	};

	//�ɽ������б�
	sequence<PSIceTransDetail> PSIceTransDetailList;

	//�������
	struct PSIceTradeDetail
	{
		string Index;		//��λ��
		int TradeDate;		//��������
		string OrderNo;		//ί�б��
		PSIceMarket Market;	//������
		string StockAccount;		//�ɶ����
		PSIceOrderType OrderType;	//ί�����
		string Code;		//��Ʊ����
		int MatchTimes;		//�ɽ�����
		int MatchVolume;	//�ɽ�����
		long MatchPrice;	//�ɽ��۸�
		long MatchAmount;	//�ɽ����
		long FundEffect;	//������
		long FeeYHS;		//ӡ��˰
		long FeeSXF;		//������
		long FeeGHF;		//������
		long FeeYJ;			//Ӷ��
		long FeeOther;		//��������
	};

	//��������б�
	sequence<PSIceTradeDetail> PSIceTradeDetailList;

	//��ѯ��ӿڣ�һ�㶼�в�ѯ��������
	interface QueryServant
	{
		//���ӽӿ�
		bool Connect(PSIceAccount account);

		//�Ͽ��ӿ�
		bool Disconnect(PSIceAccount account);

		//�ͻ���½
		int Login(PSIceAccount account, out PSIceErrorCode error);

		//�ͻ��ǳ�
		int Logout(PSIceAccount account, out PSIceErrorCode error);

		//����������Ϣ
		bool SubscribAll(PSIceAccount account);

		//ȡ������������Ϣ
		bool UnsubscribAll(PSIceAccount account);

		//��ȡ�ͻ���Ϣ
		int GetFundAccountList(PSIceAccount account, out PSIceFundAccounts fundAccounts, out PSIceErrorCode error);

		//��ȡ�ɶ��б�
		int GetStockAccountList(PSIceAccount account, out PSIceStockAccounts stockAccounts, out PSIceErrorCode error);

		//��ѯ�ֲ��б�
		int GetPositionList(PSIceAccount account, out PSIcePositionList positions, out PSIceErrorCode error);

		//��ȡ�ʽ���Ϣ
		int GetFundSummary(PSIceAccount account, out PSIceFundSummary fund, out PSIceErrorCode error);

		//��ѯ��ȯ���
		int GetSharePositionList(PSIceAccount account, out PSIceSharePositionList positions, out PSIceErrorCode error);

		//��ѯ��������ȯ
		int GetBrokerPositionList(PSIceAccount account, string index, out PSIceSharePositionList positions, out PSIceErrorCode error);

		//��ѯ����������
		int GetEnableVolume(PSIceAccount account, PSIceVolumeSearch search, out int volume, out PSIceErrorCode error);

		//��ѯ����Ʒ��Ϣ
		int GetAssureInfoList(PSIceAccount account, string index, out PSIceAssureInfoList infolist, out PSIceErrorCode error);

		//��ѯ���֤ȯ��Ϣ
		int GetObjectInfoList(PSIceAccount account, string index, out PSIceObjectInfoList infolist, out PSIceErrorCode error);

		//��ѯ���ú�Լ�б�
		int GetOrderContractList(PSIceAccount account, PSIceOrderContractSearch search, out PSIceOrderContractList contracts, out PSIceErrorCode error);

		//��ѯ����ί��״̬
		int GetTodayOrderStatusList(PSIceAccount account, PSIceOrderSearch search, out PSIceOrderStatusList statuslist, out PSIceErrorCode error);

		//��ѯ���ճɽ�״̬
		int GetTodayTransStatusList(PSIceAccount account, PSIceOrderSearch search, out PSIceTransStatusList statuslist, out PSIceErrorCode error);
		
		//��ѯί���б�, 0��ʾ���죬����20150430
		int GetOrderDetailList(int date, PSIceAccount account, string index, out PSIceOrderDetailList orderlist, out PSIceErrorCode error);

		//��ѯ�ɽ��б�
		int GetTransDetailList(int date, PSIceAccount account, string index, out PSIceTransDetailList Translist, out PSIceErrorCode error);

		//��ѯ����б�
		int GetTradeDetailList(int date, PSIceAccount account, string index, out PSIceTradeDetailList tradelist, out PSIceErrorCode error);

		
	};
};