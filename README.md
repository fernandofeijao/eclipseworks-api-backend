# Pergunta pro PO #

PO, qual o prazo de entrega? 🤣 Brincadeiras a parte, acabei levando um tempo extra pra entregar o projeto porque coincidiu essa oportunidade com a troca do meu equipamento de Windows para Mac OS e precisei investir um tempo para montar o ambiente de desenvolvimento com VS Code + PLugins, Azure Data Studio, Docker e etc. 

# Como rodar a aplicação #

Executar no terminal:

docker pull fernandofeijao/eclipseworks-app:latest
docker run -d -p 5050:80 --name container-taskmanager-api fernandofeijao/eclipseworks-app

Collection no Postman para facilitar requisições

https://api.postman.com/collections/2293133-6399bf19-4e52-45c2-b45c-9358b08136d4?access_key=PMAT-01J5TRA2MXH2NH2WQ77FT3BAP2


# Melhorias futuras #

- Adotar o padrão Decorator ou um Wrapper para separar em um camada predecessora o controle das transações no caso de endpoints de manipulação de dados
- Utilização de Aggregate para reduzir o número de injeções de dependência no construtor das classes e tornar o código mais conciso
- Não utilizaria o delete cascade no caso das foreign keys das tarefas e manteria esse controle na própria camada de serviço da Task
- Criaria uma seção no appSettings para armazenar variáveis como, por exemplo, o limite de tasks permitidas por projeto
- Nos testes unitários, daria mais atenção aos caminhos alternativos/exceptions. Além disso, separaria as etapas do "Arrange" em classes de Fixture tornando os métodos enxutos
