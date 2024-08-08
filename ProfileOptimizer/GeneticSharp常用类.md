选择算子（Selection Operators）

RouletteWheelSelection（轮盘赌选择）：
根据适应度比例选择个体，适应度高的个体被选择的概率更大。
适用于适应度差异较大的场景。

TournamentSelection（锦标赛选择）：
随机选择若干个体进行锦标赛，选择其中最优的个体。
适用于适应度差异不大的场景，简单且有效。

EliteSelection（精英选择）：
直接选择适应度最高的个体进入下一代。
保证优秀个体不会丢失，但可能导致多样性不足。

RankSelection（排名选择）：
根据个体的排名而不是适应度进行选择，排名高的个体被选择的概率更大。
解决适应度差异过大的问题，避免优秀个体的过早收敛。

StochasticUniversalSamplingSelection（随机通用采样选择）：
类似于轮盘赌选择，但更均匀地分布选择概率。
保证每个个体被选择的机会更加平衡。

=======================================================

交叉算子（Crossover Operators）

OnePointCrossover（单点交叉）：
在父代基因序列上选择一个交叉点，交换交叉点后的基因。
简单但随机性较高，可能导致较大的变异。

TwoPointCrossover（两点交叉）：
在父代基因序列上选择两个交叉点，交换这两个交叉点之间的基因。
增加了基因组合的多样性。

UniformCrossover（均匀交叉）：
根据一个概率参数（例如50%）随机决定每个基因来自哪个父代。
保持了更高的多样性，但可能破坏有利的基因组合。

OrderedCrossover（顺序交叉，OX）：
主要用于排列问题，通过部分匹配的方式交叉基因。
保证子代个体是合法的排列。

PartiallyMappedCrossover（部分映射交叉，PMX）：
适用于排列问题，通过部分映射的方式交叉基因。
保持排列的合法性。

=======================================================

变异算子（Mutation Operators）

FlipBitMutation（位翻转变异）：
对二进制编码的基因进行位翻转，0变为1，1变为0。
简单且有效，但可能导致较大的变异。

UniformMutation（均匀变异）：
随机选择一个基因位并替换为一个随机值。
适用于保持基因多样性。

TworsMutation（交换变异）：
交换基因序列中的两个基因位置。
适用于排列问题，如旅行商问题。

InversionMutation（倒置变异）：
选择一个基因子序列并将其倒置。
保持了基因序列的相对顺序。

GaussianMutation（高斯变异）：
对实数编码的基因，添加一个服从高斯分布的随机噪声。
适用于实数优化问题。


=============================

FitnessStagnationTermination：在适应度没有显著变化的情况下终止。
TimeEvolvingTermination：在运行时间达到指定值时终止。
FitnessThresholdTermination：在适应度达到指定阈值时终止。
GenerationNumberTermination：在达到指定的世代数量时终止。

AndTermination：当所有子终止条件都满足时终止。
OrTermination：当任意一个子终止条件满足时终止。