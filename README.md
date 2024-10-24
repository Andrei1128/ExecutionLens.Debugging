# ExecutionLens Debugger Package Documentation

The **ExecutionLens Debugger** is a NuGet package that allows developers to reproduce API executions based on previously logged executions. It enables the exact replay of method calls using original input and output parameters for testing business logic in debugging mode.

## Registration

To configure the ExecutionLens Debugger, you need to register it within your application using the `AddDebugger` method during service configuration.

### Example

```csharp
builder.Services.AddDebugger(config =>
{
    config.ElasticUri = elasticUri;
    config.Index = defaultIndex;
});
```

- **ElasticUri**: URI for the ElasticSearch where the logs are stored.
- **Index**: The default index where logs will be queried from.

## API Endpoint

The **ExecutionLens Debugger** exposes an endpoint through the **ReplayController**, which can only be invoked while running in debugging mode. This endpoint allows you to replay an execution based on a previously logged event using the log's ID.

### 1. Replay Execution

- **Endpoint**: `POST /Replay/{id}`

#### Description

This endpoint replays an execution by retrieving the log with the provided ID, reconstructing the execution flow, and forcing method calls to use the original input and output parameters. This ensures that the business logic is executed as it was during the original call, making it easier to test and debug.

#### Request Parameters

- **id**: The unique ID of the log to be replayed.

#### Requirements

This endpoint can only be called when the debugger is attached. If the debugger is not attached, the request will return a **BadRequest** response.

### Response

- **200 OK**: If the replay was successful.  
  Response: `"Successfully replayed!"`

- **404 Not Found**: If no log with the provided ID was found.  
  Response: `"Log with id {id} not found!"`

- **400 Bad Request**: If the debugger is not attached.  
  Response: `"Debugger is not attached!"`

- **500 Internal Server Error**: If there was an error during the replay.

## Replay Service Logic

The core functionality is handled by the **IReplayService**, which retrieves the entire log tree for the provided log ID, reconstructs the execution, and forces method calls to adhere to the original input and output parameters.

- **Execution Reproduction**: The replay service ensures that the replayed execution is identical to the original, using the original method inputs and outputs for the replayed calls.

- **Business Logic Testing**: This is ideal for testing complex business logic since it replicates real-world scenarios and data exactly as they occurred.
