#pragma once

module PortServerIce{
	exception PortServerIceException {
	};

	enum PSIceMarket {
		SH,		//����A
		SZ,		//����A
	};

	enum PSIceFundType{
		Normal,		//��ͨ�˻�
		Credit,		//�����˻�
	};

	//ί��״̬
	enum PSIceOrderState {
		Undeclared,		//δ�걨
		Declaring,		//�����걨
		Declared,		//���걨
		PendingCancel,		//�ѱ�����
		PFPendingCancel,	//���ɴ���
		PartCanceled,   //����
		Canceled,		//�ѳ�
		PartFilled,     //���ֳɽ�
		Filled,			//�ѳɽ�
		Scraped,		//�ϵ�
		Error,			//����
		Other,			//����δ֪
	};

	//ί�з�ʽ
	enum PSIceOrderType {
		Buy,	//��ͨ����
		Sell,	//��ͨ����
		CreditBuy,	//����Ʒ����
		CreditSell,	//����Ʒ����
		CashBuy,	//��������
		CashSell,   //��ȯ����
		ShareSell,  //��ȯ����
		ShareBuy,   //��ȯ��ȯ
		ShareRefund,	//��ȯ��ȯ
		CashRefund,		//�ֽ𻹿�
		None,		//δ֪
	};

	//�˻�������Ϣ
	struct PSIceAccount {
		int Id;					//�ͻ�ID
		int ClientId;			//�ͻ����ID  ��֤��fundid�ʽ��˺ţ�������client_id�ͻ����
		string ClientName;		//�ͻ����    ���㣺FID_KHH�ͻ���
		string Password;		//��������
		string ComPassword;		//ͨ������
		PSIceFundType FundType;	//�˻�����
		string BrokerName;		//����ȯ��
		string DepartmentNo;	//ȯ��Ӫҵ������
		string FundName;		//�ʽ��˺�
		string SHStockAccount;	//���йɶ��˺�
		string SZStockAccount;	//���йɶ��˺�
		string Node;			//�ڵ���Ϣ,����ip��mac��Ӳ�����
	};

	struct PSIceErrorCode
	{
		long Code;
		string Message;
	};

	
};