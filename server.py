from livereload import Server, shell
server = Server()
server.watch('theme/templates/*.html', shell('pelican', cwd='.'))
server.serve(root='output/')
