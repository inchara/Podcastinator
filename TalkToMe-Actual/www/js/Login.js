function login() {
	var endpoint = 'https://localhost:8080';
  performRequest(endpoint, '/api/session', 'POST', {
    username: "username",
    password: "password"
  }, function(data) {
    sessionId = data.result.id;
    console.log('Logged in:', sessionId);
    getCards();
  });
}
