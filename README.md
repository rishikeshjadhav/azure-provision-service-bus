# Provision Azure Service Bus Utility

This repository contains utility to provision Service Bus Queues, Topic and Subscriptions on Azure

## Pre-requisite

You will need a Service Bus namespace URL for creating Queues, Topics and Subscriptions, you can refer this [URL](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal) for creating a Service Bus namespace

## Steps to run the utility

Step 1: Clone this repository
```
git clone https://github.com/rishikeshjadhav/azure-provision-service-bus.git
```

Step 2: Open below solution in Visual Studio (2015 or higher)
```
<Repo>\NitorOSS.Azure.ProvisionServiceBus\NitorOSS.Azure.ProvisionServiceBus.sln
```

Step 3: Build the solution (This will download all the required dependencies from Nuget)

Step 4: Run the utility using Visual Studio (2015 or higher)

## Functions of utility

#### 1. Create Service Bus Queue

For details on how to provision Service Bus Queue using portal, please refer this [link](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues)



#### 2. Create Service Bus Topic

For details on how to provision Service Bus Topic using portal, please refer this [link](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions)

#### 3. Create Service Bus Topic Subscription (**_with filtering capability_**)

For details on how to provision Service Bus Topic subscription using portal, please refer this [link](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions#3-create-a-subscription-to-the-topic)


## Usage

#### For provisioning Queue, Topic and Subscriptions
Run the utility using Visual Studio and answer the questions for provisioning the componennt

#### For provisioning Subscription with filtering capability
Provide the SQL Filter expression for the subscription filter, few examples of SQL Filter expressions are,

1. Filter messages with Department as HR
```
    Department = HR
```
2. Filter messages with Role as admin or user
```
    Role = admin OR Role = user
```

For a detailed list of SQL filter syntax, please refer this [link](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-sql-filter)
