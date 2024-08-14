#### Install Missing Packages ####
list.of.packages <- c("ggplot2", "plyr", "dplyr", "rstatix", "WRS2", "car", "ggpubr", "BayesFactor", "rms", "coin", "DescTools", "grid", "forestploter", "tibble")
new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(new.packages)) install.packages(new.packages)

#### Include libraries ####
library(ggplot2)
library(plyr)
library(dplyr)
library(rstatix)
library(WRS2)
library(car)
library(ggpubr)
library(BayesFactor)
library(rms)
library(coin)
library(DescTools)
library(grid)
library(forestploter)
library(tibble)


#### Read Data ######
#Import data from files
alldata <- data.frame(read.csv("QuestionnaireData.csv", sep=";", row.names=NULL, stringsAsFactors = TRUE, header = TRUE))
subjdata <- data.frame(read.csv("FinalInterview.csv", sep=";", row.names=NULL, stringsAsFactors = TRUE, header = TRUE))

#Transform data to [-3,3] range
subjdata$SubjBetter <- 4 - subjdata$SubjBetter
subjdata$SubjUsable <- 4 - subjdata$SubjUsable
subjdata$SubjSem <- 4 - subjdata$SubjSem
subjdata$SubjLearn <- 4 - subjdata$SubjLearn

# Calculate the mean and standard error
l.model <- lm(SubjBetter ~ 1, subjdata)

# Calculate the confidence interval
confint(l.model, level=0.95)

#Remove cases where no pre questionnaire was handed in
semdata <- alldata[alldata$Seminar==1 | ( alldata$Seminar==2 & complete.cases(alldata[ , 13])),]
alldata <- alldata[complete.cases(alldata[ , 13]),]

#Rename Factor Levels
alldata$Type <- as.factor(alldata$Type)
levels(alldata$Type) <- gsub("1","Multiuser",levels(alldata$Type))
levels(alldata$Type) <- gsub("2","Singleuser",levels(alldata$Type))

alldata$Seminar <- as.factor(alldata$Seminar)
levels(alldata$Seminar) <- gsub("1","Before",levels(alldata$Seminar))
levels(alldata$Seminar) <- gsub("2","After",levels(alldata$Seminar))

semdata$Type <- as.factor(semdata$Type)
levels(semdata$Type) <- gsub("1","Multiuser",levels(semdata$Type))
levels(semdata$Type) <- gsub("2","Singleuser",levels(semdata$Type))

semdata$Seminar <- as.factor(semdata$Seminar)
levels(semdata$Seminar) <- gsub("1","Before",levels(semdata$Seminar))
levels(semdata$Seminar) <- gsub("2","After",levels(semdata$Seminar))

#Create KnowledgeGroup factor based on group identifiers
semdata$KnowledgeGroup <- NA
semdata[semdata$Seminar=="Before", ]$KnowledgeGroup <- "Class"
semdata[!is.na(semdata$Type) & semdata$Type=="Multiuser" & semdata$Seminar=="After", ]$KnowledgeGroup <- "Multi"
semdata[!is.na(semdata$Type) & semdata$Type=="Singleuser" & semdata$Seminar=="After", ]$KnowledgeGroup <- "Single"
semdata$KnowledgeGroup <- as.factor(semdata$KnowledgeGroup)

alldata$ID <- as.factor(alldata$ID)

#Calculate ARI from item scores
alldata$ARI16 <- 8 - alldata$ARI16
alldata$ARI20 <- 8 - alldata$ARI20
grep("^ARI1$", colnames(alldata))
grep("^ARI21$", colnames(alldata))
alldata$ARI <- rowMeans(alldata[,15:35])

#calculate raw TLX scores
alldata$TLX <- rowMeans(alldata[,36:41]) * 5

#calculate SUS scores
alldata$SUSOdd <- alldata$SUS1 + alldata$SUS3 + alldata$SUS5 + alldata$SUS7 + alldata$SUS9 - 5
alldata$SUSEven <- 25 - (alldata$SUS2 + alldata$SUS4 + alldata$SUS6 + alldata$SUS8 + alldata$SUS10)
alldata$SUS <- (alldata$SUSEven + alldata$SUSOdd) * 2.5

#Calculate social presence measures
alldata$Social7 <- 8 - alldata$Social7
grep("^Social1$", colnames(alldata))
alldata$InteractionPossibility <- rowMeans(alldata[,78:81])
alldata$CoPresence <- rowMeans(alldata[,82:84])

#calculate all UEQ sub-scales
alldata$UEQ1 <- alldata$UEQ1 - 4
alldata$UEQ2 <- alldata$UEQ2 - 4
alldata$UEQ3 <- 4 - alldata$UEQ3
alldata$UEQ4 <- 4 - alldata$UEQ4
alldata$UEQ5 <- 4 - alldata$UEQ5
alldata$UEQ6 <- alldata$UEQ6 - 4
alldata$UEQ7 <- alldata$UEQ7 - 4
alldata$UEQ8 <- alldata$UEQ8 - 4
alldata$UEQ9 <- 4 - alldata$UEQ9
alldata$UEQ10 <- 4 - alldata$UEQ10
alldata$UEQ11 <- alldata$UEQ11 - 4
alldata$UEQ12 <- 4 - alldata$UEQ12
alldata$UEQ13 <- alldata$UEQ13 - 4
alldata$UEQ14 <- alldata$UEQ14 - 4
alldata$UEQ15 <- alldata$UEQ15 - 4
alldata$UEQ16 <- alldata$UEQ16 - 4
alldata$UEQ17 <- 4 - alldata$UEQ17
alldata$UEQ18 <- 4 - alldata$UEQ18
alldata$UEQ19 <- 4 - alldata$UEQ19
alldata$UEQ20 <- alldata$UEQ20 - 4
alldata$UEQ21 <- 4 - alldata$UEQ21
alldata$UEQ22 <- alldata$UEQ22 - 4
alldata$UEQ23 <- 4 - alldata$UEQ23
alldata$UEQ24 <- 4 - alldata$UEQ24
alldata$UEQ25 <- 4 - alldata$UEQ25
alldata$UEQ26 <- alldata$UEQ26 - 4

alldata$UEQAttractiveness <- (alldata$UEQ1 + alldata$UEQ12 + alldata$UEQ14 + alldata$UEQ16 + alldata$UEQ24 + alldata$UEQ25) / 6
alldata$UEQAPerspicuity <- (alldata$UEQ2 + alldata$UEQ4 + alldata$UEQ13 + alldata$UEQ21) / 4
alldata$UEQEfficiency <- (alldata$UEQ9 + alldata$UEQ20 + alldata$UEQ22 + alldata$UEQ23) / 4
alldata$UEQDependability <- (alldata$UEQ8 + alldata$UEQ11 + alldata$UEQ17 + alldata$UEQ19) / 4
alldata$UEQStimulation <- (alldata$UEQ5 + alldata$UEQ6 + alldata$UEQ7 + alldata$UEQ18) / 4
alldata$UEQNovelty <- (alldata$UEQ3 + alldata$UEQ10 + alldata$UEQ15 + alldata$UEQ26) / 4

alldata$UEQ <- rowMeans(alldata[,42:67])

#Knowledge Test Scores
semdata$GEK <- semdata$postGEK - semdata$preGEK
semdata$HEK <- semdata$postHEK - semdata$preHEK
semdata$ET <- semdata$postGEK + semdata$postHEK

#semdataHEK <- semdata[['KnowledgeGroup', 'preGEK', 'postGEK']].copy()

#Data Frames for knowledge test analyses
semdataGEK <- semdata[c("ID","KnowledgeGroup","preGEK","postGEK")]
semdataGEK <- semdataGEK[complete.cases(semdataGEK[ , 3]),]
semdataGEK <- gather(semdataGEK, condition, measurement, preGEK:postGEK, factor_key=TRUE)

semdataHEK <- semdata[c("ID","KnowledgeGroup","preHEK","postHEK")]
semdataHEK <- semdataHEK[complete.cases(semdataHEK[ , 3]),]
semdataHEK <- gather(semdataHEK, condition, measurement, preHEK:postHEK, factor_key=TRUE)

semdataGEK2 <- semdata[c("ID","Seminar","preGEK","postGEK")]
semdataGEK2 <- semdataGEK2[complete.cases(semdataGEK2[ , 3]),]
semdataGEK2 <- gather(semdataGEK2, condition, measurement, preGEK:postGEK, factor_key=TRUE)

semdataHEK2 <- semdata[c("ID","Seminar","preHEK","postHEK")]
semdataHEK2 <- semdataHEK2[complete.cases(semdataHEK2[ , 3]),]
semdataHEK2 <- gather(semdataHEK2, condition, measurement, preHEK:postHEK, factor_key=TRUE)


semdataNA <- semdata[complete.cases(semdata[ , 86]),]
groupAdata <- alldata[alldata$Seminar=="After" & complete.cases(alldata[ , 86]),]

groupAdata$GEK <- groupAdata$postGEK - groupAdata$preGEK
groupAdata$HEK <- groupAdata$postHEK - groupAdata$preHEK
groupAdata$ET <- groupAdata$postGEK + groupAdata$postHEK

groupAdataGEK <- groupAdata[c("ID","Type","preGEK","postGEK")]
groupAdataGEK <- groupAdataGEK[complete.cases(groupAdataGEK[ , 3]),]
groupAdataGEK <- gather(groupAdataGEK, condition, measurement, preGEK:postGEK, factor_key=TRUE)

groupAdataHEK <- groupAdata[c("ID","Type","preHEK","postHEK")]
groupAdataHEK <- groupAdataHEK[complete.cases(groupAdataHEK[ , 3]),]
groupAdataHEK <- gather(groupAdataHEK, condition, measurement, preHEK:postHEK, factor_key=TRUE)

alldata$GEK <- alldata$postGEK - alldata$preGEK
alldata$HEK <- alldata$postHEK - alldata$preHEK
alldata$Combi <- interaction(alldata$Seminar,alldata$Type)

semdataHEK$KnowledgeGroup <- factor(semdataHEK$KnowledgeGroup, levels = c("Class", "Single","Multi"))

####HEK Seminar Interaction Plot####
HEKInteraction <- ddply(semdataHEK2, c("Seminar", "condition"), summarise,
                        HEK.n=length(measurement[!is.na(measurement)]),
                        HEK.mean=mean(measurement,na.rm = TRUE),
                        HEK.sd=sd(measurement,na.rm = TRUE),
                        HEK.se=HEK.sd/sqrt(HEK.n))

HEKInteraction$Seminar <- factor(HEKInteraction$Seminar, levels = c("After", "Before"))
HEKInteraction$condition <- factor(HEKInteraction$condition, levels = c("preHEK", "postHEK"))

(ggplot(HEKInteraction,aes(x=Seminar,y=HEK.mean, fill=interaction(Seminar,condition)))+
    coord_cartesian(ylim=c(0,10))+
    scale_fill_manual(values=c("#F29999","#A1C8F7","#DC3F3D","#417DDD"),
                      labels=c("Before Seminar", "After Seminar")) +
    scale_x_discrete(labels=c("MR-based\nLearning", "Traditional\nLearning")) +
    scale_y_continuous(breaks=seq(0, 10, 2))+
    geom_bar (stat="identity", size=.35,width=.5, position=position_dodge(.75),color=NA)+
    geom_errorbar(aes(ymin=HEK.mean-HEK.se,ymax=HEK.mean+HEK.se),width=.075, position=position_dodge(.75),colour="#000000", size=0.2)+
    #geom_text(aes(label=c("Pre", "Post", "Pre", "Post"), y=-0.25), position=position_dodge(.75), size=1, angle=0, hjust=0.5) +  # Adding custom labels
    labs(
      y="Pre & Post HEK Scores",
      #tag = "a)",
      fill="Time of Test"
    )+
    theme_bw()+
    theme(
      text=element_text(size=7,family="serif"),
      #axis.text.x = element_text(angle=30,vjust=.5),
      #plot.title = element_blank(),
      plot.tag.position = c(0.025, 0.975),
      axis.title.x = element_blank(),
      plot.title = element_blank(),
      legend.position = "none",
      panel.grid.major=element_line(size=0.2),
      panel.grid.minor=element_line(size=0.2),
      plot.margin=grid::unit(c(4.0,5.5,0,5.5),"point",),
      axis.line.y=element_line(size=0.2),        
      axis.ticks.y=element_line(size=0.2),       
      axis.ticks.x=element_line(size=0.0),       
      panel.border=element_rect(size=0.2)        
    ))
ggsave("Plots/HEK2.png", unit="cm",height=3,width=4.237125,dpi=500) 



####GET Seminar Interaction Plot####
GEKInteraction <- ddply(semdataGEK2, c("Seminar", "condition"), summarise,
                        GEK.n=length(measurement[!is.na(measurement)]),
                        GEK.mean=mean(measurement,na.rm = TRUE),
                        GEK.sd=sd(measurement,na.rm = TRUE),
                        GEK.se=GEK.sd/sqrt(GEK.n))

GEKInteraction$Seminar <- factor(GEKInteraction$Seminar, levels = c("After", "Before"))
GEKInteraction$condition <- factor(GEKInteraction$condition, levels = c("preGEK", "postGEK"))

(ggplot(GEKInteraction,aes(x=Seminar,y=GEK.mean, fill=interaction(Seminar,condition)))+
    coord_cartesian(ylim=c(0,10))+
    scale_fill_manual(values=c("#F29999","#A1C8F7","#DC3F3D","#417DDD"),
                      labels=c("Before Seminar", "After Seminar")) +
    scale_x_discrete(labels=c("MR-based\nLearning", "Traditional\nLearning")) +
    scale_y_continuous(breaks=seq(0, 10, 2))+
    geom_bar (stat="identity", size=.35,width=.5, position=position_dodge(.75),color=NA)+
    geom_errorbar(aes(ymin=GEK.mean-GEK.se,ymax=GEK.mean+GEK.se),width=.075, position=position_dodge(.75),colour="#000000", size=0.2)+
    #geom_text(aes(label=c("Pre", "Post", "Pre", "Post"), y=-0.25), position=position_dodge(.75), size=1, angle=0, hjust=0.5) +  # Adding custom labels
    labs(
      y="Pre & Post GEK Scores",
      #tag = "a)",
      fill="Time of Test"
    )+
    theme_bw()+
    theme(
      text=element_text(size=7,family="serif"),
      #axis.text.x = element_text(angle=30,vjust=.5),
      #plot.title = element_blank(),
      plot.tag.position = c(0.025, 0.975),
      axis.title.x = element_blank(),
      plot.title = element_blank(),
      legend.position = "none",
      panel.grid.major=element_line(size=0.2),
      panel.grid.minor=element_line(size=0.2),
      plot.margin=grid::unit(c(4.0,5.5,0,5.5),"point",),
      axis.line.y=element_line(size=0.2),        
      axis.ticks.y=element_line(size=0.2),      
      axis.ticks.x=element_line(size=0.0),       
      panel.border=element_rect(size=0.2)       
    ))
ggsave("Plots/GET2.png", unit="cm",height=3,width=4.237125,dpi=500) 



####HEK Type Interaction Plot####
HEKInteractionType <- ddply(groupAdataHEK, c("Type", "condition"), summarise,
                        HEK.n=length(measurement[!is.na(measurement)]),
                        HEK.mean=mean(measurement,na.rm = TRUE),
                        HEK.sd=sd(measurement,na.rm = TRUE),
                        HEK.se=HEK.sd/sqrt(HEK.n))

HEKInteractionType$Type <- factor(HEKInteractionType$Type, levels = c("Singleuser", "Multiuser"))
HEKInteractionType$condition <- factor(HEKInteractionType$condition, levels = c("preHEK", "postHEK"))

(ggplot(HEKInteractionType,aes(x=Type,y=HEK.mean, fill=interaction(Type,condition)))+
    coord_cartesian(ylim=c(0,10))+
    scale_fill_manual(values=c("#FFD8AE","#B5EAE3","#FFB255","#74C1B8"),
                      labels=c("Before Seminar", "After Seminar")) +
    scale_x_discrete(labels=c("ILE", "CLE")) +
    geom_bar (stat="identity", size=.35,width=.5, position=position_dodge(.75),color=NA)+
    geom_errorbar(aes(ymin=HEK.mean-HEK.se,ymax=HEK.mean+HEK.se),width=.075, position=position_dodge(.75),colour="#000000", size=0.2)+
    scale_y_continuous(breaks=seq(0, 10, 2))+
    #geom_text(aes(label=c("Pre", "Post", "Pre", "Post"), y=-0.25), position=position_dodge(.75), size=1, angle=0, hjust=0.5) +  # Adding custom labels
    labs(
      y="Pre & Post HEK Scores",
      #tag = "a)",
      fill="Time of Test"
    )+
    theme_bw()+
    theme(
      text=element_text(size=7,family="serif"),
      #axis.text.x = element_text(angle=30,vjust=.5),
      #plot.title = element_blank(),
      plot.tag.position = c(0.025, 0.975),
      axis.title.x = element_blank(),
      plot.title = element_blank(),
      legend.position = "none",
      panel.grid.major=element_line(size=0.2),
      panel.grid.minor=element_line(size=0.2),
      plot.margin=grid::unit(c(4.0,5.5,0,5.5),"point",),
      axis.line.y=element_line(size=0.2),        
      axis.ticks.y=element_line(size=0.2),      
      axis.ticks.x=element_line(size=0.0),       
      panel.border=element_rect(size=0.2)        
    ))
ggsave("Plots/HEK3.png", unit="cm",height=3,width=4.237125,dpi=500) 



####GET Type Interaction Plot####
GEKInteractionType <- ddply(groupAdataGEK, c("Type", "condition"), summarise,
                            GEK.n=length(measurement[!is.na(measurement)]),
                            GEK.mean=mean(measurement,na.rm = TRUE),
                            GEK.sd=sd(measurement,na.rm = TRUE),
                            GEK.se=GEK.sd/sqrt(GEK.n))

GEKInteractionType$Type <- factor(GEKInteractionType$Type, levels = c("Singleuser", "Multiuser"))
GEKInteractionType$condition <- factor(GEKInteractionType$condition, levels = c("preGEK", "postGEK"))

(ggplot(GEKInteractionType,aes(x=Type,y=GEK.mean, fill=interaction(Type,condition)))+
    coord_cartesian(ylim=c(0,10))+
    scale_fill_manual(values=c("#FFD8AE","#B5EAE3","#FFB255","#74C1B8"),
                      labels=c("Before Seminar", "After Seminar")) +
    scale_x_discrete(labels=c("ILE", "CLE")) +
    geom_bar (stat="identity", size=.35,width=.5, position=position_dodge(.75),color=NA)+
    geom_errorbar(aes(ymin=GEK.mean-GEK.se,ymax=GEK.mean+GEK.se),width=.075, position=position_dodge(.75),colour="#000000", size=0.2)+
    scale_y_continuous(breaks=seq(0, 10, 2))+
    #geom_text(aes(label=c("Pre", "Post", "Pre", "Post"), y=-0.25), position=position_dodge(.75), size=1, angle=0, hjust=0.5) +  # Adding custom labels
    labs(
      y="Pre & Post GEK Scores",
      #tag = "a)",
      fill="Time of Test"
    )+
    theme_bw()+
    theme(
      text=element_text(size=7,family="serif"),
      #axis.text.x = element_text(angle=30,vjust=.5),
      #plot.title = element_blank(),
      plot.tag.position = c(0.025, 0.975),
      axis.title.x = element_blank(),
      plot.title = element_blank(),
      legend.position = "none",
      panel.grid.major=element_line(size=0.2),
      panel.grid.minor=element_line(size=0.2),
      plot.margin=grid::unit(c(4.0,5.5,0,5.5),"point",),
      axis.line.y=element_line(size=0.2),        
      axis.ticks.y=element_line(size=0.2),       
      axis.ticks.x=element_line(size=0.0),       
      panel.border=element_rect(size=0.2)        
    ))
ggsave("Plots/GET3.png", unit="cm",height=3,width=4.237125,dpi=500) 



####Parametric Assumptions####

leveneTest(ARI ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(ARI)


leveneTest(TLX ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(TLX)


leveneTest(SUS ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(SUS)


leveneTest(UEQ ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(UEQ)


leveneTest(InteractionPossibility ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(InteractionPossibility)


leveneTest(CoPresence ~ Type, data=alldata)
alldata %>% group_by(Type) %>% shapiro_test(CoPresence)

leveneTest(GEK ~ Seminar, data=semdataNA)
semdataNA %>% group_by(Seminar) %>% shapiro_test(GEK)


leveneTest(HEK ~ Seminar, data=semdataNA)
semdataNA %>% group_by(Seminar) %>% shapiro_test(HEK)


leveneTest(GEK ~ Type, data=groupAdata)
groupAdata %>% group_by(Type) %>% shapiro_test(GEK)


leveneTest(HEK ~ Type, data=groupAdata)
groupAdata %>% group_by(Type) %>% shapiro_test(HEK)


####YUEN Testst####
yuen(GEK~Seminar, data=semdataNA)
yuen(GEK~Type, data=groupAdata)
yuen(HEK~Seminar, data=semdataNA)
yuen(HEK~Type, data=groupAdata)
yuen(SUS~Type, data=alldata)
yuen(TLX~Type, data=alldata)
yuen(UEQ~Type, data=alldata)
yuen(InteractionPossibility~Type, data=alldata)
yuen(CoPresence~Type, data=alldata)


####Bayes Factors####
ttestBF(x = semdataNA$GEK[semdataNA$Seminar=="Before"], y = semdataNA$GEK[semdataNA$Seminar=="After"], paired = FALSE) #-> 0.249647 moderate
ttestBF(x = groupAdata$GEK[groupAdata$Type=="Singleuser"], y = groupAdata$GEK[groupAdata$Type=="Multiuser"], paired = FALSE) #-> 0.249647 moderate
ttestBF(x = semdataNA$HEK[semdataNA$Seminar=="Before"], y = semdataNA$HEK[semdataNA$Seminar=="After"], paired = FALSE) #-> 0.249647 moderate
ttestBF(x = groupAdata$HEK[groupAdata$Type=="Singleuser"], y = groupAdata$HEK[groupAdata$Type=="Multiuser"], paired = FALSE) #-> 0.249647 moderate
ttestBF(x = alldata$SUS[alldata$Type=="Singleuser"], y = alldata$SUS[alldata$Type=="Multiuser"], paired = FALSE) #-> 0.249647 moderate
ttestBF(x = alldata$TLX[alldata$Type=="Singleuser"], y = alldata$TLX[alldata$Type=="Multiuser"], paired = FALSE) #-> 0.3754215 anecdotal
ttestBF(x = alldata$UEQ[alldata$Type=="Singleuser"], y = alldata$UEQ[alldata$Type=="Multiuser"], paired = FALSE) #-> 0.2762099 moderate
ttestBF(x = alldata$InteractionPossibility[alldata$Type=="Singleuser"], y = alldata$InteractionPossibility[alldata$Type=="Multiuser"], paired = FALSE) #-> 0.3754215 anecdotal
ttestBF(x = alldata$CoPresence[alldata$Type=="Singleuser"], y = alldata$CoPresence[alldata$Type=="Multiuser"], paired = FALSE) #-> 0.2762099 moderate

#### Forest Plot####
groupAdata$GEKscale <- groupAdata$GEK + 10
groupAdata$HEKscale <- groupAdata$HEK + 10

GEKCI<-MeanDiffCI(x = groupAdata$GEKscale[groupAdata$Type=="Multiuser"], y = groupAdata$GEKscale[groupAdata$Type=="Singleuser"])
HEKCI<-MeanDiffCI(x = groupAdata$HEKscale[groupAdata$Type=="Multiuser"], y = groupAdata$HEKscale[groupAdata$Type=="Singleuser"])

susCI<-MeanDiffCI(x = alldata$SUS[alldata$Type=="Multiuser"], y = alldata$SUS[alldata$Type=="Singleuser"])
tlxCI<-MeanDiffCI(x = alldata$TLX[alldata$Type=="Multiuser"], y = alldata$TLX[alldata$Type=="Singleuser"])
ariCI<-MeanDiffCI(x = alldata$ARI[alldata$Type=="Multiuser"], y = alldata$ARI[alldata$Type=="Singleuser"])
interactionCI<-MeanDiffCI(x = alldata$InteractionPossibility[alldata$Type=="Multiuser"], y = alldata$InteractionPossibility[alldata$Type=="Singleuser"])
copresenceCI<-MeanDiffCI(x = alldata$CoPresence[alldata$Type=="Multiuser"], y = alldata$CoPresence[alldata$Type=="Singleuser"])
ueqCI<-MeanDiffCI(x = alldata$UEQ[alldata$Type=="Multiuser"]+4, y = alldata$UEQ[alldata$Type=="Singleuser"]+4)

singleMeans <- c(mean(alldata$SUS[alldata$Type=="Singleuser"]), mean(alldata$TLX[alldata$Type=="Singleuser"]), mean(alldata$ARI[alldata$Type=="Singleuser"]),mean(alldata$InteractionPossibility[alldata$Type=="Singleuser"]), mean(alldata$CoPresence[alldata$Type=="Singleuser"]), mean(alldata$UEQ[alldata$Type=="Singleuser"]))
singleMeans <- round(singleMeans, digits = 3)

multiMeans <- c(mean(alldata$SUS[alldata$Type=="Multiuser"]), mean(alldata$TLX[alldata$Type=="Multiuser"]), mean(alldata$ARI[alldata$Type=="Multiuser"]),mean(alldata$InteractionPossibility[alldata$Type=="Multiuser"]), mean(alldata$CoPresence[alldata$Type=="Multiuser"]), mean(alldata$UEQ[alldata$Type=="Multiuser"]))
multiMeans <- round(multiMeans, digits = 3)

# Calculate the mean and standard error
l.model <- lm(SubjBetter ~ 1, subjdata)
# Calculate the confidence interval
subjBetterCI <- confint(l.model, level=0.95)

l.model <- lm(SubjUsable ~ 1, subjdata)
# Calculate the confidence interval
subjUsableCI <- confint(l.model, level=0.95)

l.model <- lm(SubjLearn ~ 1, subjdata)
# Calculate the confidence interval
subjLearnCI <- confint(l.model, level=0.95)


subjMeans <- c(mean(subjdata$SubjBetter, na.rm = TRUE), mean(subjdata$SubjUsable,na.rm = TRUE), mean(subjdata$SubjLearn,na.rm = TRUE))
subjMeans <- round(subjMeans, digits = 3)


ciData <- tibble::tibble( mean  = c(round(GEKCI[1]/20, digits = 3), round(HEKCI[1]/20, digits = 3),round(susCI[1]/100, digits = 3), round(tlxCI[1]/100, digits = 3),round(ueqCI[1]/7, digits = 3), round(interactionCI[1]/7, digits = 3),round(copresenceCI[1]/7, digits = 3),subjMeans[1]/3, subjMeans[2]/3, subjMeans[3]/3),
                          lower = c(round(GEKCI[2]/20, digits = 3), round(HEKCI[2]/20, digits = 3),round(susCI[2]/100, digits = 3), round(tlxCI[2]/100, digits = 3),round(ueqCI[2]/7, digits = 3), round(interactionCI[2]/7, digits = 3),round(copresenceCI[2]/7, digits = 3),subjBetterCI[1,1]/3, subjUsableCI[1,1]/3, subjLearnCI[1,1]/3),
                          upper = c(round(GEKCI[3]/20, digits = 3), round(HEKCI[3]/20, digits = 3),round(susCI[3]/100, digits = 3), round(tlxCI[3]/100, digits = 3),round(ueqCI[3]/7, digits = 3), round(interactionCI[3]/7, digits = 3),round(copresenceCI[3]/7, digits = 3),subjBetterCI[1,2]/3, subjUsableCI[1,2]/3, subjLearnCI[1,2]/3),
                          "Variable" = c("GEK-Diff", "HEK-Diff", "SUS", "TLX", "UEQ", "SI*", "CP*", "GP", "EU", "LP"),
                          #"Variable (SF)" = c("SUS (100)", "TLX (100)", , "Social Interaction (7)", "Co-Presence (7)", "UEQ (7)", "General Preference (3)", "More Usable (3)", "Learning Preference (3)"),
                          size = c(0.45, 0.45, 0.45, 0.45, 0.45, 0.45, 0.45, 0.45, 0.45, 0.45),
                          #"Single-User" = append(singleMeans, c("","","")),
                          #"Multi-User" = append(multiMeans, c("","","")),)
                          "Single-User" = c("", "", "", "", "", "", "", "", "", ""),
                          "Multi-User" = c("", "", "", "", "", "", "", "", "", ""),
                          "SF" = c("20", "20", "100", "100", "7", "7", "7", "3", "3", "3"))

ciData$` ` <- paste(rep(" ", 55), collapse = " ")


tm <- forest_theme(base_size = 5.5,
                   # Confidence interval point shape, line type/color/width
                   ci_pch = 23,
                   ci_col = "black",
                   ci_fill = "black",
                   ci_alpha = 1,
                   ci_lty = 1,
                   ci_lwd =1.5,
                   ci_Theight = 0.2, # Set a T end at the end of CI 
                   # Reference line width/type/color
                   #refline_lwd = gpar(lwd = 1, lty = "dashed", col = "grey20"),
                   # Vertical line width/type/color
                   vertline_lwd = 1,
                   vertline_lty = "dashed",
                   vertline_col = "grey80",
                   # Change summary color for filling and borders
                   #summary_fill = "#4575b4",
                   #summary_col = "#4575b4",
                   # Footnote font size/face/color
                   #footnote_gp = gpar(cex = 0.6, fontface = "italic", col = "blue")
                   )

p <- forest(ciData[,c(4, 8, 9)],
            est = ciData$mean,
            lower = ciData$lower, 
            upper = ciData$upper,
            sizes = ciData$size,
            ci_column = 3,
            ref_line = 0,
            arrow_lab = c("ILE superior", "CLE superior"),
            xlim = c(-0.5, 0.5),
            ticks_at = c(-0.5, -0.25, 0, 0.25, 0.5),
            #footnote = "This is my first test.",
            theme = tm)

# Print plot
plot(p)

ggsave("./Plots/forest.png", plot = p, unit="cm",height=6,width=8.47415,dpi=300)

#### Data Frames for Table ####
tabledata<-alldata
tabledata$Seminar <- factor(tabledata$Seminar, levels = c("After", "Before"))
tabledata$Type <- factor(tabledata$Type, levels = c("Singleuser", "Multiuser"))
Means <- ddply(tabledata,c("Seminar", "Type"),summarise,
               GEK.mean=round(mean(GEK,na.rm = TRUE), digits = 2),
               GEK.sd=round(sd(GEK,na.rm = TRUE), digits = 2),
               
               HEK.mean=round(mean(HEK,na.rm = TRUE), digits = 2),
               HEK.sd=round(sd(HEK,na.rm = TRUE), digits = 2),
               
                                   SUS.mean=round(mean(SUS,na.rm = TRUE), digits = 2),
                                   SUS.sd=round(sd(SUS,na.rm = TRUE), digits = 2),
                                   
                                   TLX.mean=round(mean(TLX,na.rm = TRUE), digits = 2),
                                   TLX.sd=round(sd(TLX,na.rm = TRUE), digits = 2),
               
               UEQ.mean=round(mean(UEQ,na.rm = TRUE), digits = 2),
               UEQ.sd=round(sd(UEQ,na.rm = TRUE), digits = 2),
               
               InteractionPossibility.mean=round(mean(InteractionPossibility,na.rm = TRUE), digits = 2),
               InteractionPossibility.sd=round(sd(InteractionPossibility,na.rm = TRUE), digits = 2),
               
               CoPresence.mean=round(mean(CoPresence,na.rm = TRUE), digits = 2),
               CoPresence.sd=round(sd(CoPresence,na.rm = TRUE), digits = 2))

Means2 <- ddply(tabledata,c("Type"),summarise,
                GEK.mean=round(mean(GEK,na.rm = TRUE), digits = 2),
                GEK.sd=round(sd(GEK,na.rm = TRUE), digits = 2),
                
                HEK.mean=round(mean(HEK,na.rm = TRUE), digits = 2),
                HEK.sd=round(sd(HEK,na.rm = TRUE), digits = 2),
                
               SUS.mean=round(mean(SUS,na.rm = TRUE), digits = 2),
               SUS.sd=round(sd(SUS,na.rm = TRUE), digits = 2),
               
               TLX.mean=round(mean(TLX,na.rm = TRUE), digits = 2),
               TLX.sd=round(sd(TLX,na.rm = TRUE), digits = 2),
               
               UEQ.mean=round(mean(UEQ,na.rm = TRUE), digits = 2),
               UEQ.sd=round(sd(UEQ,na.rm = TRUE), digits = 2),
               
               InteractionPossibility.mean=round(mean(InteractionPossibility,na.rm = TRUE), digits = 2),
               InteractionPossibility.sd=round(sd(InteractionPossibility,na.rm = TRUE), digits = 2),
               
               CoPresence.mean=round(mean(CoPresence,na.rm = TRUE), digits = 2),
               CoPresence.sd=round(sd(CoPresence,na.rm = TRUE), digits = 2))

subjMeans <- c(mean(subjdata$SubjBetter, na.rm = TRUE), mean(subjdata$SubjUsable,na.rm = TRUE), mean(subjdata$SubjLearn,na.rm = TRUE))
subjMeans <- round(subjMeans, digits = 3)

subjDataMeans <- ddply(subjdata,.(),summarise,
                      SubjBetter.mean=round(mean(SubjBetter,na.rm = TRUE), digits = 2),
                      SubjBetter.sd=round(sd(SubjBetter,na.rm = TRUE), digits = 2),
                      
                      SubjUsable.mean=round(mean(SubjUsable,na.rm = TRUE), digits = 2),
                      SubjUsable.sd=round(sd(SubjUsable,na.rm = TRUE), digits = 2),
                      
                      SubjLearn.mean=round(mean(SubjLearn,na.rm = TRUE), digits = 2),
                      SubjLearn.sd=round(sd(SubjLearn,na.rm = TRUE), digits = 2))

text <- "& "
for(j in seq(from=3, to=ncol(Means), by=2)) {
  for(i in seq(from=1, to=nrow(Means), by=1)) {       # for-loop over columns
    text <- paste(text, format(Means[i, j], nsmall = 2), " [", format(Means[i, j+1], nsmall = 2), "] & ", sep="")
  }
  text <- paste(text, format(Means2[1, j-1], nsmall = 2), " [", format(Means2[1, j], nsmall = 2), "] & ", sep="")
  text <- paste(text, format(Means2[2, j-1], nsmall = 2), " [", format(Means2[2, j], nsmall = 2), "] & ", sep="")
  text <- paste(text, "!")
}
text


MeansParticipants <- ddply(tabledata,c("Seminar", "Type"),summarise,
               count=length(TLX[!is.na(TLX)]),
               countF=length(Sex[Sex=="1"]),
               age=median(Age),
               Tech=median(Tech),
               MR=median(MR),
               Games=median(Games),
               Aknow=median(Aknowledge),
               Hknow=median(Hknowledge))

MeansParticipants2 <- ddply(tabledata,c("Type"),summarise,
                            count=length(TLX[!is.na(TLX)]),
                            countF=length(Sex[Sex=="1"]),
                            age=median(Age),
                            Tech=median(Tech),
                            MR=median(MR),
                            Games=median(Games),
                            Aknow=median(Aknowledge),
                            Hknow=median(Hknowledge))


text <- "& "
for(j in seq(from=3, to=ncol(MeansParticipants), by=1)) {
  for(i in seq(from=1, to=nrow(MeansParticipants), by=1)) {       # for-loop over columns
    text <- paste(text, format(MeansParticipants[i, j], nsmall = 2), " & ", sep="")
  }
  text <- paste(text, format(MeansParticipants2[1, j-1], nsmall = 2), " & ", sep="")
  text <- paste(text, format(MeansParticipants2[2, j-1], nsmall = 2),  " & ", sep="")
  text <- paste(text, "!")
}
text


