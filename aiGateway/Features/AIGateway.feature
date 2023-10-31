Feature: AI Gateway



Scenario: OpenAI Proxy GPT 3.5 Turbo
	Given que eu tenha os dados servicesId e apiKey
	And quero criar o modelo "gpt-35-turbo" 
	When envio a requisicao para o endpoint do AI Gateway
	Then entao vejo o status ok e o atributo completion_tokens maior que Zero 

	#consegui fazer funcionar, tem que recuperar os dados e fazer funcionar
