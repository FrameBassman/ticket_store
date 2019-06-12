# dev
build-dev:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.development.yml \
		build ${ARGS}

start-dev:
	chmod 600 ./Proxy/traefik/certs/acme.json
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.development.yml \
		up --build

stop-dev:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.development.yml \
		down

# test
start-test:
	chmod 600 ./Proxy/traefik/certs/acme.json
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.test.yml \
		up --build -d

stop-test:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.test.yml \
		down

# prod
build-prod:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.production.yml \
		build ${ARGS}

stop-prod:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.production.yml \
		down

start-prod:
	chmod 600 ./Proxy/traefik/certs/acme.json
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.production.yml \
		pull
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.yml \
		-f Deploy/docker-compose.production.yml \
		up -d

push:
	docker login
	docker-compose -f Deploy/docker-compose.production.yml push

pull:
	docker login
	docker-compose -f Deploy/docker-compose.production.yml pull

# clean dungling images/volumes
docker-cleanup:
	docker volume rm $$(docker volume ls -qf dangling=true)
	docker rmi $$(docker images -a | grep "^<none>" | awk '{print $$3}')
	docker rmi $$(docker images -a --filter=dangling=true -q)
	docker rm $$(docker ps --filter=status=exited --filter=status=created -q)

db-dev:
	docker exec -it postgres psql postgresql://store:W6TTT3WY4Nzqpj5z@localhost:5432/store
	
cleanup-db-dev:
	docker exec -it postgres psql postgresql://store:W6TTT3WY4Nzqpj5z@localhost:5432/store -c 'DROP SCHEMA public CASCADE;' -c '\q'

migrate-dev:
	docker exec -it store_api dotnet ef database update

migrate-test:
	docker exec -it store_api dotnet ef database update

# dev
ngrok:
	ngrok http 5000 --bind-tls true --region eu
