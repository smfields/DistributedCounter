import grpc from 'k6/net/grpc';
import { sleep } from 'k6';

export const options = {
  // A number specifying the number of VUs to run concurrently.
  vus: 100,
  // A string specifying the total duration of the test run.
  duration: '30s',
};

const client = new grpc.Client();
client.load([], 'counter_service.proto');

function create_counter() {
  connect();

  const response = client.invoke('distributed_counter.protos.CounterService/CreateCounter', {
    initial_value: 0
  });
  
  client.close();

  console.log(JSON.stringify(response.message));
  return response.message.counterId;
}

function connect() {
  client.connect('localhost:50051', {
    plaintext: true
  });
}

export function setup() {
  return create_counter();
}

export default function(data) {
  client.connect('localhost:50051', {
    plaintext: true
  });
  
  client.invoke('distributed_counter.protos.CounterService/IncrementCounter', {
    counter_id: data,
    increment_amount: 1
  });
  
  client.close();
  // sleep(1);
}
