<h2>A Basic Genetic Algorithm</h2>
<P>
    A genetic algorithm (GA) is best suited to a problem that doesn't require an exact answer, 
    just a good (or better) answer. In addition, it is important that for a genetic algorithm 
    to work, we need to be able to determine how good an answer is.
</P>
<P>
    In summary, genetic algorithms are best suited to problems that can be summed up by the 
    the terms <em>'near enough is good enough'</em> and <em>'knowing a good answer when you see one'</em>.
</P>
<h3>Fitness and Termination</h3>
<p>
    Fitness refers to the assessment of a solution provided by the genetic algorithm. 
    For example if the GAF is being used to optimise the shape of a fan blade, the genetic 
    algorithm will constantly present a collection of fan blade parameters to the fitness 
    function for evaluation. If you are a developer using the GAF, the fitness function is 
    where your effort should be focused.
</p>
<p>
    The GAF simply requires that you create a fitness function in the following form. The 
    method name is unimportant, but the signature is. The contents of this function are 
    determined by the developer.
</p>

<pre><code>private double CalculateFitness(Chromosome chromosome)
{
    //calculate fitness 
    //...

    return fitness;
}</code></pre>
<p>
    This function is passed to the GAF as a delegate during initialisation 
    and will be called by the GAF when the GAF needs to evaluate a solution. 
    Solutions in this context are passed to the fitness function using the 
    Chromosome type. The value returned by the fitness function should be set 
    to a real number between 0 and 1, with 1 being the fittest.
</p>
<p>
    Stopping the genetic algorithm, hopefully once it has a suitable solution 
    to the problem, is carried out by the Terminate function. This function is 
    also provided by the developer and passed to the GAF as a delegate. It will 
    be called periodically by the GAF. Returning 'true' from this function will 
    terminate the running genetic algorithm. It should take the following form.
</p>
<pre><code>private bool TerminateFunction(Population population, 
                                int currentGeneration, 
                                long currentEvaluation) 
{ 
    //determine criteria on which to terminate
    //...

    return result; 
}</code></pre>
<p>
    These two functions, as delegates, will be called by the GAF as needed.
    but with the parameters populated. For example, the Fitness function will pass
    a populated Chromosome object to be evaluated. Similarly the terminate function
    will be populated with the current population, the current generation and the number 
    of evaluations so far. The following example would terminate the algorithm after
    40000 evaluations.
</p>
<pre><code>private bool TerminateFunction(Population population, 
                                int currentGeneration, 
                                long currentEvaluation) 
{ 
    //example termination criterion 
    return currentEvaluation >= 40000; 
}</code></pre>

<h3>Population</h3>      
<p>
    Before the GA can be initialised, a population needs to be defined. Full details of the Population object
    are shown in the following sections of this documentation. However, in order to show how simple this
    can be, the code below initialtes a random binary population of 100 solutions each with a binary
    chromosome length of 44 bits.
</p>
<pre><code>//randomly generated population 
population = new Population(100, 44);</code></pre>
<p>
    Once the population has been defined, the genetic algorithm can be initialised.
</p>
<pre><code>var ga = new GeneticAlgorithm(population, FitnessFunction);</code></pre>

<h3>Subscribing to Generation and Run Events</h3>
<p>
    To monitor progress of the algorithm, several events are available. The two main events
    are <strong>OnGenerationComplete</strong> and <strong>OnRunComplete</strong>.
</p>
<pre><code>ga.OnGenerationComplete += ga_OnGenerationComplete;
ga.OnRunComplete += ga_OnRunComplete;</code></pre>

<h3>Genetic Operators</h3>
<p>
    Before the GA can be run, Genetic Operators need to be determined and added to the Operators
    collection. The GAF include many operators, these are detailed in the following sections of 
    this documentation. The code shown below configures and adds three operators to the GA 
    (Elite, Crossover, Binary Mutate) and adds them to the Operators collection..
</p>
<pre><code>elite = new Elite(5%); 

crossover = new Crossover(0.85)
{ 
    CrossoverType = CrossoverType.DoublePoint
}; 

mutate = new BinaryMutate(0.04);

ga.Operators.Add(elite); 
ga.Operators.Add(crossover); 
ga.Operators.Add(mutate);</code></pre> 
<p>
    Once all of the components are in place, the GA can be run.
</p>
<pre><code>ga.Run(TerminateFunction);</code></pre>

<h3>Full Example</h3>
<p>
    To see a concrete example of how the GA fits together, please see
    the example, <a href="/gaf/section/501">Solving the Binary F6 Function</a>.
</p>

<a href="/gaf/section/102" class="btn-u pull-right">Next ></a>


