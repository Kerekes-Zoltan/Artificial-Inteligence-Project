using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AI_Proiect3
{
    public partial class Form1 : Form
    {

        private bool first = true;
        private bool firstNorm = true;
        private List<MushroomData> listaInputs = new List<MushroomData>();
        private List<List<decimal>> atributeNormalizate = new List<List<decimal>>();
        private BackgroundWorker myWorker = new BackgroundWorker();
        //am luat un string, am pus toate posibilitatile pentru a vedea cat de lungi sunt, sa calculez la normalizare pentru interval [0,1]
        private string[] resultVF = new string[] { "p", "e"};
        private string[] cap_shapeVF = new string[] { "b", "c", "x", "f", "k", "s"};
        private string[] cap_surfaceVF = new string[] { "f", "g", "y", "s"};
        private string[] cap_colorVF = new string[] { "n", "b", "c", "g", "r", "p", "u", "e", "w", "y"};
        private string[] bruisesVF = new string[] { "t", "f"};
        private string[] odorVF = new string[] { "a", "l", "c", "y", "f", "m", "n", "p", "s"};
        private string[] gill_attachmentVF = new string[] { "a", "d", "f", "n"};
        private string[] gill_spacingVF = new string[] { "c", "w", "d"};
        private string[] gill_sizeVF = new string[] { "b", "n"};
        private string[] gill_colorVF = new string[] { "k", "n", "b", "h", "g", "r", "o", "p", "u", "e", "w", "y"};
        private string[] stalk_shapeVF = new string[] { "e", "t" };
        private string[] stalk_rootVF = new string[] { "b", "c", "u", "e", "z", "r", "?"};
        private string[] stalk_surface_above_ringVF = new string[] { "f", "y", "k", "s"};
        private string[] stalk_surface_below_ringVF = new string[] { "f", "y", "k", "s"};
        private string[] stalk_color_above_ringVF = new string[] { "n", "b", "c", "g", "o", "p", "e", "w", "y"};
        private string[] stalk_color_below_ringVF = new string[] { "n", "b", "c", "g", "o", "p", "e", "w", "y"};
        private string[] veil_typeVF = new string[] { "p", "u" };
        private string[] veil_colorVF = new string[] { "n", "o", "w", "y" };
        private string[] ring_numberVF = new string[] { "n", "o", "t"};
        private string[] ring_typeVF = new string[] { "c", "e", "f", "l", "n", "p", "s", "z" };
        private string[] spore_print_colorVF = new string[] { "k", "n", "b", "h", "r", "o", "u", "w", "y"};
        private string[] populationVF = new string[] { "a", "c", "n", "s", "v", "y"};
        private string[] habitatVF = new string[] { "g", "l", "m", "p", "u", "w", "d"};

        private static float e = 2.71f;
        private decimal nrhidden = 1;
        private bool stopClicked = false;
        private List<NumericUpDown> UpDownHidden = new List<NumericUpDown>();
        public float[,,] weightValue = new float[5, 10000, 10000];
        Random rand = new Random();
        public int[] vectorHiddenLayer = new int[5];
        public float[] valInputuri = new float[25];
        public int counter = 0;
        public float[] nextLayerInputs = new float[10000];//cand trimit dintr-un layer in altul, sa pot trimite la celalalt layer
        public float[,] valoriHidden1 = new float[10000, 10000];//stochez valorile si wieght ul
        public float[,] valoriHidden2 = new float[10000, 10000];
        public float[,] valoriHidden3 = new float[10000, 10000];
        public float[] valoriOutput = new float[10000];
        public static float teta = 0f;
        public static float AorG = 1.0f;
        public static int nrAtribute = 22;
        public int epoca = 1;
        public float sum = 0;

        public Form1()
        {
            InitializeComponent();
            //dataGridViewIN.Hide();
            dataGridViewNORM.Hide();
            panel1.Hide();
            addControls(1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonIN.Click += new EventHandler(buttonIN_Click);
            buttonNORM.Click += new EventHandler(buttonNORM_Click);
            buttonINV.Click += new EventHandler(buttonINV_Click);

            myWorker.RunWorkerCompleted += WorkerCheckIfCompleted;
            myWorker.DoWork += WorkerCalculation;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for(int i=1;i<=UpDownHidden[0].Value;i++)
            {
                for(int j=0;j<=listaInputs.Count;j++)
                {
                    weightValue[1, i, j] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                }
            }

            if (nrhidden >= 2)
            {
                for (int i = 1; i <= UpDownHidden[1].Value; i++)
                {
                    for (int j = 0; j <= UpDownHidden[0].Value; j++)
                    {
                        weightValue[2, i, j] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                    }
                }

                for (int i = 0; i <= UpDownHidden[1].Value; i++)
                {
                    weightValue[4, 1, i] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                }
            }
            if (nrhidden == 3)
            {
                for (int i = 1; i <= UpDownHidden[2].Value; i++)
                {
                    for (int j = 0; j <= UpDownHidden[1].Value; j++)
                    {
                        weightValue[3, i, j] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                    }
                }

                for (int i = 0; i <= UpDownHidden[2].Value; i++)
                {
                    weightValue[4, 1, i] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                }
            }
            if(nrhidden == 1)
            {
                for (int i = 0; i <= UpDownHidden[0].Value; i++)
                {
                    weightValue[4, 1, i] = (float)(-1.0f + (rand.NextDouble() * (1.0f - -1.0f)));
                }
            }
        }

        private void buttonIN_Click(object sender, EventArgs e)
        {
            dataGridViewNORM.Hide();
            panel1.Hide();
            dataGridViewIN.Show();

            if (first)
            {
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Ali3n\Desktop\AI_Proiect3\agaricus-lepiota.data");
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] words = lines[i].Split(',');
                    MushroomData date = new MushroomData(words);
                    listaInputs.Add(date);
                }
                for (int i = 0; i < lines.Length; i++)
                {
                    DataGridViewRow newRow = (DataGridViewRow)dataGridViewIN.Rows[0].Clone();
                    newRow.Cells[0].Value = listaInputs[i].result;
                    newRow.Cells[1].Value = listaInputs[i].cap_shape;
                    newRow.Cells[2].Value = listaInputs[i].cap_surface;
                    newRow.Cells[3].Value = listaInputs[i].cap_color;
                    newRow.Cells[4].Value = listaInputs[i].bruises;
                    newRow.Cells[5].Value = listaInputs[i].odor;
                    newRow.Cells[6].Value = listaInputs[i].gill_attachment;
                    newRow.Cells[7].Value = listaInputs[i].gill_spacing;
                    newRow.Cells[8].Value = listaInputs[i].gill_size;
                    newRow.Cells[9].Value = listaInputs[i].gill_color;
                    newRow.Cells[10].Value = listaInputs[i].stalk_shape;
                    newRow.Cells[11].Value = listaInputs[i].stalk_root;
                    newRow.Cells[12].Value = listaInputs[i].stalk_surface_above_ring;
                    newRow.Cells[13].Value = listaInputs[i].stalk_surface_below_ring;
                    newRow.Cells[14].Value = listaInputs[i].stalk_color_above_ring;
                    newRow.Cells[15].Value = listaInputs[i].stalk_color_below_ring;
                    newRow.Cells[16].Value = listaInputs[i].veil_type;
                    newRow.Cells[17].Value = listaInputs[i].veil_color;
                    newRow.Cells[18].Value = listaInputs[i].ring_number;
                    newRow.Cells[19].Value = listaInputs[i].ring_type;
                    newRow.Cells[20].Value = listaInputs[i].spore_print_color;
                    newRow.Cells[21].Value = listaInputs[i].population;
                    newRow.Cells[22].Value = listaInputs[i].habitat;
                    dataGridViewIN.Rows.Add(newRow);
                }
            }
            first = false;
        }

        private void buttonNORM_Click(object sender, EventArgs e)
        {
            dataGridViewIN.Hide();
            panel1.Hide();
            dataGridViewNORM.Show();
            //date normalizate-transforma datele de intrare in numere pentru functii
            if (firstNorm)
            {
                for (int i = 0; i < listaInputs.Count; i++)
                {
                    DataGridViewRow newRow = (DataGridViewRow)dataGridViewNORM.Rows[0].Clone();
                    List<decimal> lista = new List<decimal>();

                    newRow.Cells[0].Value = ((Decimal)1 / resultVF.Length) / 2 + ((Decimal)1 / resultVF.Length) * Array.IndexOf(resultVF, listaInputs[i].result);
                    lista.Add((decimal)newRow.Cells[0].Value);
                    newRow.Cells[1].Value = ((Decimal)1 / cap_shapeVF.Length) / 2 + ((Decimal)1 / cap_shapeVF.Length) * Array.IndexOf(cap_shapeVF, listaInputs[i].cap_shape);
                    lista.Add((decimal)newRow.Cells[1].Value);
                    newRow.Cells[2].Value = ((Decimal)1 / cap_surfaceVF.Length) / 2 + ((Decimal)1 / cap_surfaceVF.Length) * Array.IndexOf(cap_surfaceVF, listaInputs[i].cap_surface);
                    lista.Add((decimal)newRow.Cells[2].Value);
                    newRow.Cells[3].Value = ((Decimal)1 / cap_colorVF.Length) / 2 + ((Decimal)1 / cap_colorVF.Length) * Array.IndexOf(cap_colorVF, listaInputs[i].cap_color);
                    lista.Add((decimal)newRow.Cells[3].Value);
                    newRow.Cells[4].Value = ((Decimal)1 / bruisesVF.Length) / 2 + ((Decimal)1 / bruisesVF.Length) * Array.IndexOf(bruisesVF, listaInputs[i].bruises);
                    lista.Add((decimal)newRow.Cells[4].Value);
                    newRow.Cells[5].Value = ((Decimal)1 / odorVF.Length) / 2 + ((Decimal)1 / odorVF.Length) * Array.IndexOf(odorVF, listaInputs[i].odor);
                    lista.Add((decimal)newRow.Cells[5].Value);
                    newRow.Cells[6].Value = ((Decimal)1 / gill_attachmentVF.Length) / 2 + ((Decimal)1 / gill_attachmentVF.Length) * Array.IndexOf(gill_attachmentVF, listaInputs[i].gill_attachment);
                    lista.Add((decimal)newRow.Cells[6].Value);
                    newRow.Cells[7].Value = ((Decimal)1 / gill_spacingVF.Length) / 2 + ((Decimal)1 / gill_spacingVF.Length) * Array.IndexOf(gill_spacingVF, listaInputs[i].gill_spacing);
                    lista.Add((decimal)newRow.Cells[7].Value);
                    newRow.Cells[8].Value = ((Decimal)1 / gill_sizeVF.Length) / 2 + ((Decimal)1 / gill_sizeVF.Length) * Array.IndexOf(gill_sizeVF, listaInputs[i].gill_size);
                    lista.Add((decimal)newRow.Cells[8].Value);
                    newRow.Cells[9].Value = ((Decimal)1 / gill_colorVF.Length) / 2 + ((Decimal)1 / gill_colorVF.Length) * Array.IndexOf(gill_colorVF, listaInputs[i].gill_color);
                    lista.Add((decimal)newRow.Cells[9].Value);
                    newRow.Cells[10].Value = ((Decimal)1 / stalk_shapeVF.Length) / 2 + ((Decimal)1 / stalk_shapeVF.Length) * Array.IndexOf(stalk_shapeVF, listaInputs[i].stalk_shape);
                    lista.Add((decimal)newRow.Cells[10].Value);
                    newRow.Cells[11].Value = ((Decimal)1 / stalk_rootVF.Length) / 2 + ((Decimal)1 / stalk_rootVF.Length) * Array.IndexOf(stalk_rootVF, listaInputs[i].stalk_root);
                    lista.Add((decimal)newRow.Cells[11].Value);
                    newRow.Cells[12].Value = ((Decimal)1 / stalk_surface_above_ringVF.Length) / 2 + ((Decimal)1 / stalk_surface_above_ringVF.Length) * Array.IndexOf(stalk_surface_above_ringVF, listaInputs[i].stalk_surface_above_ring);
                    lista.Add((decimal)newRow.Cells[12].Value);
                    newRow.Cells[13].Value = ((Decimal)1 / stalk_surface_below_ringVF.Length) / 2 + ((Decimal)1 / stalk_surface_below_ringVF.Length) * Array.IndexOf(stalk_surface_below_ringVF, listaInputs[i].stalk_surface_below_ring);
                    lista.Add((decimal)newRow.Cells[13].Value);
                    newRow.Cells[14].Value = ((Decimal)1 / stalk_color_above_ringVF.Length) / 2 + ((Decimal)1 / stalk_color_above_ringVF.Length) * Array.IndexOf(stalk_color_above_ringVF, listaInputs[i].stalk_color_above_ring);
                    lista.Add((decimal)newRow.Cells[14].Value);
                    newRow.Cells[15].Value = ((Decimal)1 / stalk_color_below_ringVF.Length) / 2 + ((Decimal)1 / stalk_color_below_ringVF.Length) * Array.IndexOf(stalk_color_below_ringVF, listaInputs[i].stalk_color_below_ring);
                    lista.Add((decimal)newRow.Cells[15].Value);
                    newRow.Cells[16].Value = ((Decimal)1 / veil_typeVF.Length) / 2 + ((Decimal)1 / veil_typeVF.Length) * Array.IndexOf(veil_typeVF, listaInputs[i].veil_type);
                    lista.Add((decimal)newRow.Cells[16].Value);
                    newRow.Cells[17].Value = ((Decimal)1 / veil_colorVF.Length) / 2 + ((Decimal)1 / veil_colorVF.Length) * Array.IndexOf(veil_colorVF, listaInputs[i].veil_color);
                    lista.Add((decimal)newRow.Cells[17].Value);
                    newRow.Cells[18].Value = ((Decimal)1 / ring_numberVF.Length) / 2 + ((Decimal)1 / ring_numberVF.Length) * Array.IndexOf(ring_numberVF, listaInputs[i].ring_number);
                    lista.Add((decimal)newRow.Cells[18].Value);
                    newRow.Cells[19].Value = ((Decimal)1 / ring_typeVF.Length) / 2 + ((Decimal)1 / ring_typeVF.Length) * Array.IndexOf(ring_typeVF, listaInputs[i].ring_type);
                    lista.Add((decimal)newRow.Cells[19].Value);
                    newRow.Cells[20].Value = ((Decimal)1 / spore_print_colorVF.Length) / 2 + ((Decimal)1 / spore_print_colorVF.Length) * Array.IndexOf(spore_print_colorVF, listaInputs[i].spore_print_color);
                    lista.Add((decimal)newRow.Cells[20].Value);
                    newRow.Cells[21].Value = ((Decimal)1 / populationVF.Length) / 2 + ((Decimal)1 / populationVF.Length) * Array.IndexOf(populationVF, listaInputs[i].population);
                    lista.Add((decimal)newRow.Cells[21].Value);
                    newRow.Cells[22].Value = ((Decimal)1 / habitatVF.Length) / 2 + ((Decimal)1 / habitatVF.Length) * Array.IndexOf(habitatVF, listaInputs[i].habitat);
                    lista.Add((decimal)newRow.Cells[22].Value);
                    Console.WriteLine(lista[21]);
                    dataGridViewNORM.Rows.Add(newRow);
                    atributeNormalizate.Add(lista);
                }
            }
            firstNorm = false;
        }

        private void buttonINV_Click(object sender, EventArgs e)
        {
            dataGridViewIN.Hide();
            dataGridViewNORM.Hide();
            panel1.Show();

        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < nrhidden; i++)
            {
                vectorHiddenLayer[i] = (int)UpDownHidden[i].Value;
            }
            stopClicked = false;
            myWorker.RunWorkerAsync();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopClicked = true;
        }
        //calcule gresite
        private void WorkerCalculation(object sender, DoWorkEventArgs e)
        {
            if (counter == 5800)
            {
                Console.WriteLine("Epoca: " + epoca);
                epoca++;
                counter = 0;
                for(int i=0;i<5800;i++)
                {
                    sum = sum + (float)atributeNormalizate[i][0] - valoriOutput[3];
                }
                sum = sum / 5800;
                Console.WriteLine(sum);
                sum = 0;
            }
            for(int i=0;i<nrAtribute;i++)
            {
                valInputuri[i] = (float)atributeNormalizate[counter][i];
            }
            float rezFinal;
            for (int j = 1; j <= vectorHiddenLayer[0]; j++)
            {
                for (int k = 0; k < nrAtribute; k++)
                {
                    nextLayerInputs[k] = valInputuri[k] * (float)weightValue[1, j, k];
                    //nextLayerInputs[k] = valFinale[0, k];
                }
                valoriHidden1[j, 1] = Suma(nrAtribute, nextLayerInputs);
                rezFinal = valoriHidden1[j, 1] - teta;
                
                valoriHidden1[j, 2] = Sigmoidala(rezFinal, AorG, teta);
                valoriHidden1[j, 3] = valoriHidden1[j, 2];
            }
            if (nrhidden == 1)
            {
                for (int j = 1; j <= 1; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[0]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden1[k + 1, 3] * (float)weightValue[4, j, k];
                        //nextLayerInputs[k] = valFinale[3, k];
                        //Console.Write("NextLayerInputs: " + nextLayerInputs[k] + " ");
                    }
                    //Console.WriteLine();
                    valoriOutput[1] = Suma(vectorHiddenLayer[0], nextLayerInputs);
                    rezFinal = valoriOutput[1];
                    valoriOutput[2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriOutput[3] = valoriOutput[2];
                }
            }
            else if (nrhidden == 2)
            {
                for (int j = 1; j <= vectorHiddenLayer[1]; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[0]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden1[k + 1, 3] * (float)weightValue[2, j, k];
                        //nextLayerInputs[k] = valFinale[1, k];
                    }
                    valoriHidden2[j, 1] = Suma(vectorHiddenLayer[0], nextLayerInputs);
                    rezFinal = valoriHidden2[j, 1];
                    valoriHidden2[j, 2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriHidden2[j, 3] = valoriHidden2[j, 2];
                }
                for (int j = 1; j <= 1; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[1]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden2[k + 1, 3] * (float)weightValue[4, j, k];
                        //nextLayerInputs[k] = valFinale[3, k];
                        //Console.Write("NextLayerInputs: " + nextLayerInputs[k] + " ");
                    }
                    //Console.WriteLine();
                    valoriOutput[1] = Suma(vectorHiddenLayer[1], nextLayerInputs);
                    rezFinal = valoriOutput[1];
                    valoriOutput[2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriOutput[3] = valoriOutput[2];
                }
            }
            else if (nrhidden == 3)
            {
                for (int j = 1; j <= vectorHiddenLayer[1]; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[0]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden1[k + 1, 3] * (float)weightValue[2, j, k];
                        //nextLayerInputs[k] = valFinale[1, k];
                    }
                    valoriHidden2[j, 1] = Suma(vectorHiddenLayer[0], nextLayerInputs);
                    rezFinal = valoriHidden2[j, 1];
                    valoriHidden2[j, 2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriHidden2[j, 3] = valoriHidden2[j, 2];
                }
                for (int j = 1; j <= vectorHiddenLayer[2]; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[1]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden3[k + 1, 3] * (float)weightValue[3, j, k];
                        //nextLayerInputs[k] = valFinale[2, k];
                    }
                    valoriHidden3[j, 1] = Suma(vectorHiddenLayer[1], nextLayerInputs);
                    rezFinal = valoriHidden3[j, 1];
                    valoriHidden3[j, 2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriHidden3[j, 3] = valoriHidden3[j, 2];
                }
                for (int j = 1; j <= 1; j++)
                {
                    for (int k = 0; k < vectorHiddenLayer[2]; k++)
                    {
                        nextLayerInputs[k] = valoriHidden3[k + 1, 3] * (float)weightValue[4, j, k];
                        //nextLayerInputs[k] = valFinale[3, k];
                        //Console.Write("NextLayerInputs: " + nextLayerInputs[k] + " ");
                    }
                    //Console.WriteLine();
                    valoriOutput[1] = Suma(vectorHiddenLayer[2], nextLayerInputs);
                    rezFinal = valoriOutput[1];
                    valoriOutput[2] = Sigmoidala(rezFinal, AorG, teta);
                    valoriOutput[3] = valoriOutput[2];
                }
            }
        }

        private void WorkerCheckIfCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (stopClicked)
            {
                Console.WriteLine("Stopped");
            }
            else
            {
                counter++;
                myWorker.RunWorkerAsync();
            }
        }

        private void addControls(int i)
        {
            NumericUpDown newNumericUpDownHidden = new NumericUpDown
            {
                Size = new Size(50, 15),
                Location = new Point(100, 30 * Convert.ToInt32(i)-25),
                Minimum = 1,
            };

            System.Windows.Forms.Label newLabelHidden = new System.Windows.Forms.Label
            {
                Text = "Hidden" + Convert.ToString(i - 1) + ":",
                Font = new Font("Times New Roman", 10),
                Size = new Size(100, 15),
                Location = new Point(10, 30 * Convert.ToInt32(i)-25),
            };

            UpDownHidden.Add(newNumericUpDownHidden);
            hiddenLayers.Controls.Add(newNumericUpDownHidden);

            hiddenLayers.Controls.Add(newLabelHidden);
        }

        private void nrhiddenlayer_ValueChanged(object sender, EventArgs e)
        {
            if (nrhidden != nrhiddenlayer.Value && nrhiddenlayer.Value != '1')
            {
                if (nrhidden < nrhiddenlayer.Value)
                {
                    Console.WriteLine("up");
                    Console.WriteLine("nrhidden=" + nrhidden);
                    Console.WriteLine("nrhiddenlayer=" + nrhiddenlayer.Value);
                    if (nrhiddenlayer.Value - nrhidden > 1)
                    {
                        for (int i = (int)nrhidden + 1; i <= nrhiddenlayer.Value; i++)
                        {
                            addControls(i);
                        }
                    }
                    else
                    {
                        addControls((int)nrhiddenlayer.Value);
                    }
                    nrhidden = nrhiddenlayer.Value;
                }
                else
                {
                    Console.WriteLine("down");
                    Console.WriteLine("nrhidden=" + nrhidden);
                    Console.WriteLine("nrhiddenlayer=" + nrhiddenlayer.Value);
                    if (nrhidden - nrhiddenlayer.Value > 1)
                    {
                        for (int i = (int)nrhidden - 1; i >= nrhiddenlayer.Value; i--)
                        {
                            Console.WriteLine("asd");
                            hiddenLayers.Controls.RemoveAt((int)i * 2);
                            hiddenLayers.Controls.RemoveAt((int)i * 2);
                        }
                    }
                    else
                    {
                        nrhidden = nrhiddenlayer.Value;
                        hiddenLayers.Controls.RemoveAt((int)nrhidden * 2);
                        hiddenLayers.Controls.RemoveAt((int)nrhidden * 2);
                    }
                    nrhidden = nrhiddenlayer.Value;
                    Console.WriteLine("nrhidden=" + nrhidden);
                }

            }
        }

        private float Suma(int nrcomp, float[] vectorInputuri)
        {
            float suma = 0;
            for (int i = 0; i < nrcomp; i++)
            {
                suma += vectorInputuri[i];
            }
            return suma;
        }

        private static float Sigmoidala(float rezult, float g, float teta)
        {
            double rez = 1 / (1 + Math.Pow(e, g * (-1) * (rezult - teta)));
            float rez1 = (float)rez;
            return rez1;
        }
    }
}
