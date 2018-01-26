#pragma once

#include "Common.ice"

module PortServerIce{

	struct PSIceOrder
	{
		int BatchId;		//ί������
		string ClOrderId;	//�ͻ���OrderId
		PSIceMarket Market;	//��Ʊ�г�
		PSIceOrderType OrderType;  //ί������
		string Code;		//��Ʊ����
		long Price;			//ί�м۸�
		int Volume;			//ί������
		string Node;		//�ڵ���Ϣ
	};

	struct PSIceOrderOut
	{
		string ClOrderId;	//�ͻ���OrderId
		string OrderNo;	//ί�б��
		int BatchId;		//ί������
	};

	struct PSIceOrderCancel
	{
		PSIceOrderType OrderType;  //ί������
		string ClOrderId;	//�ͻ���OrderId
		int BatchId;		//ί������
		string OrderNo;		//ί�б��
		PSIceMarket Market; //������
		string Code;		//��Ʊ����
		int CancelVolume;	//��������
		string Node;		//�ڵ���Ϣ
	};

	struct PSIceOrderCancelOut
	{
		string ClOrderId;	//�ͻ���OrderId
		string OrderNo;		//ί�б��
	};

	["clr:class"]struct PSIceCashRefund
	{
		long FundEffect;	//�������
		string Node;		//�ڵ���Ϣ
	};

	["clr:class"]struct PSIceCashRefundOut
	{
		long Effect;			//ʵ�ʻ����
	};

	struct PSIceShareRefund
	{
		string ClOrderId;	//�ͻ���OrderId
		int BatchId;		//ί������
		string ContractNo;	//��Լ���
		PSIceMarket Market;	//��Ʊ�г�
		string Code;		//��Ʊ����
		int Volume;			//��ȯ����
		string Node;		//�ڵ���Ϣ
	};

	struct PSIceShareRefundOut
	{
		string ClOrderId;	//�ͻ���OrderId
		string OrderNo;		//ί�б��
		int BatchId;		//ί������
	};

	//������ӿڣ��޲�ѯ��������
	interface TradeServant
	{
		//�ŵ�
		int SendOrder(PSIceAccount account, PSIceOrder order, out PSIceOrderOut orderout, out PSIceErrorCode error);

		//����
		int CancelOrder(PSIceAccount account, PSIceOrderCancel cancel, out PSIceOrderCancelOut cancelout, out PSIceErrorCode error);

		//��ȯ��ȯ
		int RefundShare(PSIceAccount account, PSIceShareRefund share, out PSIceShareRefundOut refundout, out PSIceErrorCode error);

		//�ֽ𻹿�,����ʵ�ʻ����
		int RefundCash(PSIceAccount account, PSIceCashRefund cash, out PSIceCashRefundOut refundout, out PSIceErrorCode error);

	};
};