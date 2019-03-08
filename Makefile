build:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.development.yml \
		build ${ARGS}

start:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.development.yml \
		up --build

stop-dev:
	docker-compose \
		--project-directory=${PWD} \
		--project-name=ticket_store \
		-f Deploy/docker-compose.development.yml \
		down

# clean dungling images/volumes
docker-cleanup:
	docker volume rm $$(docker volume ls -qf dangling=true)
	docker rmi $$(docker images -a | grep "^<none>" | awk '{print $$3}')
	docker rmi $$(docker images -a --filter=dangling=true -q)
	docker rm $$(docker ps --filter=status=exited --filter=status=created -q)

db-dev:
	docker exec -it postgres psql postgresql://store:W6TTT3WY4Nzqpj5z@localhost:5432/store

db-prod:
	docker exec -it postgres psql postgresql://store:W6TTT3WY4Nzqpj5z@82.202.236.173:5432/store

