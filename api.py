from flask import Flask


app = Flask(__name__)

@app.route('/')
def index():
    with open("C:\\Users\\minef\\OneDrive\\Área de Trabalho\\cadastro de produto webhook e api\\produto\\bin\\Debug\\produtos.txt", "r") as arquivo:
        conteudo = arquivo.read()
    return conteudo

if __name__ == '__main__':
    app.run(debug=True)
