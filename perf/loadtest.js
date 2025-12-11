import http from 'k6/http';

export default function () {
  http.get('https://bpapp-staging-d5d8a0gcejghg9g8.westeurope-01.azurewebsites.net/');
}
