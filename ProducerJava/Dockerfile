FROM maven:3.9.2-eclipse-temurin-17 as base

FROM base as build
COPY src /home/app/src
COPY pom.xml /home/app
RUN mvn -f /home/app/pom.xml clean package

FROM base as final
COPY --from=build /home/app/target/ProducerJava-1.0-SNAPSHOT.jar /usr/local/lib/ProducerJava.jar
ENTRYPOINT ["java","-jar","/usr/local/lib/ProducerJava.jar"]