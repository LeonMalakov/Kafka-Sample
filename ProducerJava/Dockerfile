FROM maven:3.9.3-amazoncorretto-17 as build
COPY src /home/app/src
COPY pom.xml /home/app
RUN mvn -f /home/app/pom.xml clean package

FROM openjdk:17.0.1-jdk-slim as final
COPY --from=build /home/app/target/ProducerJava-1.0-SNAPSHOT-jar-with-dependencies.jar /usr/local/lib/ProducerJava.jar
ENTRYPOINT ["java","-jar","/usr/local/lib/ProducerJava.jar"]