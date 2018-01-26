#pragma once

#include "Common.ice"
#include <Ice/Identity.ice>

module PortServerIce{

	enum PSIceCallbackReportType {
		ReportConfirmed,	//�걨ȷ��
		ReportError,		//�걨����
		ReportScraped 	//�걨�ϵ�
	};

	enum PSIceCallbackCancelType 
	{
		CancelSuccess,	//�����ɹ�
		CancelFail,		//����ʧ��
		CancelScraped		//�����ϵ�
	};

	//ί�гɽ��ر�
	struct PSIceCallbackTransaction
	{
		string OrderNo;		//ί�б��
		string TransNo;		//�ɽ����
		PSIceMarket Market;		//������
		string StockAccount;	//�ɶ����
		int Side;			//������־
		string Code;		//��Ʊ����
		long OrderPrice;	//ί�м۸�
		int OrderVolume;	//ί������
		int FilledVolume;	//���γɽ�����
		long FilledPrice;	//���γɽ��۸�
		long FilledAmount;	//���γɽ����
		long TotalAmount;	//�ܳɽ������ʳɽ������
		int TotalVolume;	//�ܳɽ����������ʳɽ������
		string Message;		//�ɽ�ժҪ
	};

	//ί���걨�ر�
	struct PSIceCallbackReport
	{
		string OrderNo;		//ί�б��
		PSIceMarket Market;	//������
		string StockAccount; //�ɶ����
		int Side;			//������־
		string Code;		//��Ʊ����
		PSIceCallbackReportType Type;	//��Ϣ����
		int ReportCode;		//�걨�������
		string ReportMessage;	//�걨�����Ϣ
	};

	//ί�г���ȷ�ϻر�
	struct PSIceCallbackCancel
	{
		string OrderNo;		//ί�б��
		PSIceMarket Market;	//������
		string StockAccount;//�ɶ����
		string Code;		//��Ʊ����
		int OrderVolume;	//ί������
		int CancelVolume;	//��������
		int FilledVolume;	//�ɽ�����
		PSIceCallbackCancelType Type;	//��������
		int CancelCode;		//�����������
		string CancelMessage; //���������Ϣ
	};


	//���׻ص����Ͷ�
	interface TradeCallbackSender
	{
		void Heartbeat();

		void Start(string name, Ice::Identity ident);

		void Stop();
	};

	//���׻ص�����
	interface TradeCallbackReceiver
	{
		void Notify();

		void CallbackTransaction(PSIceCallbackTransaction transaction);

		void CallbackReport(PSIceCallbackReport report);

		void CallbackCancel(PSIceCallbackCancel cancel);
	};
};