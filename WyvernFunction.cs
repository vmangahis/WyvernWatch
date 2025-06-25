using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using WyvernWatch.Interfaces;


namespace WyvernWatch;

public class WyvernFunction
{
    private readonly ICourierService courierService;
    private readonly IAPIClient api;


    public WyvernFunction(ICourierService ms, IAPIClient a)
    {
        courierService = ms;
        api = a;
    }

    [Function("WyvernFunction")]
    public async Task Run([TimerTrigger("0 59 23 * * *")] TimerInfo myTimer)
    {
       string summary = await api.FetchAsync();
       courierService.SendEmail(summary);
    }
}