/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System.Linq;
using GAF;
using GAF.Operators;

namespace CustomOperators
{
	public enum AutoMutateFactor
	{
		None = 1,
		Factor5 = 5,
		Factor10 = 10,
		Factor20 = 20,
		Factor50 = 50
	}

    public class AutoMutate : BinaryMutate
    {
        private AutoMutateFactor _autoMutationFactorS;
        private readonly object _syncLock = new object();
        private int _geneCount;

        public AutoMutate(double mutationProbability)
            : base(mutationProbability)
        {
        }

        public override void Invoke(Population currentPopulation, ref Population newPopulation,
          FitnessFunction fitnessFunctionDelegate)
        {
            _geneCount = newPopulation.ChromosomeLength;
            base.Invoke(currentPopulation, ref newPopulation, fitnessFunctionDelegate);
        }

        protected override void Mutate(Chromosome child, double mutationProbability)
        {
            //adjust and scale for AutoMutate Factor based on the value of the last gene
            var newMutationProbability = 0.0;
            var nonPhenotypeGene = child.Genes.ElementAt(_geneCount - 1);

            if (nonPhenotypeGene.BinaryValue == 1)
            {
                newMutationProbability = mutationProbability * (int)AutoMutationFactor;
            }
            else
            {
                newMutationProbability = mutationProbability;
            }

            base.Mutate(child, newMutationProbability);

        }

        public AutoMutateFactor AutoMutationFactor
        {
            get
            {
                lock (_syncLock)
                {
                    return _autoMutationFactorS;
                }
            }
            set
            {
                lock (_syncLock)
                {
                    _autoMutationFactorS = value;
                }
            }
        }
    }
		


}
