using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Proiect3
{
    class MushroomData
    {	
		public string cap_shape, cap_surface, cap_color, odor, gill_attachment, gill_spacing, gill_size, gill_color, stalk_shape, stalk_root;
		public string stalk_surface_above_ring, stalk_surface_below_ring, stalk_color_above_ring, stalk_color_below_ring, veil_type, veil_color;
		public string ring_number, ring_type, spore_print_color, population, habitat, result;
		public string bruises;
		//clasa care creaza cate un obiect sa pot folosi datele
		public MushroomData(string[] inputs)
		{
			this.result = inputs[0];
			this.cap_shape = inputs[1];
			this.cap_surface = inputs[2];
			this.cap_color = inputs[3];
			this.bruises = inputs[4];
			this.odor = inputs[5];
			this.gill_attachment = inputs[6];
			this.gill_spacing = inputs[7];
			this.gill_size = inputs[8];
			this.gill_color = inputs[9];
			this.stalk_shape = inputs[10];
			this.stalk_root = inputs[11];
			this.stalk_surface_above_ring = inputs[12];
			this.stalk_surface_below_ring = inputs[13];
			this.stalk_color_above_ring = inputs[14];
			this.stalk_color_below_ring = inputs[15];
			this.veil_type = inputs[16];
			this.veil_color = inputs[17];
			this.ring_number = inputs[18];
			this.ring_type = inputs[19];
			this.spore_print_color = inputs[20];
			this.population = inputs[21];
			this.habitat = inputs[22];
		}
	}
}
