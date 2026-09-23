using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaburetkaPlugin
{
    public partial class Form1 : Form
    {
        // ===== ЭЛЕМЕНТЫ ИНТЕРФЕЙСА =====
        // Поля ввода для параметров
        private TextBox txtH;
        private TextBox txtW;
        private TextBox txtD;
        private TextBox txtd;
        private TextBox txtt;

        // Кнопка «Построить»
        private Button btnBuild;

        // ===== КОНСТРУКТОР =====
        public Form1()
        {
            InitializeComponent();
            SetupForm();
        }

        // ===== НАСТРОЙКА ФОРМЫ =====
        // Создаёт все элементы интерфейса программно.
        private void SetupForm()
        {
            // --- Настройки самой формы ---
            this.Text = "Разработка плагина «Табуретка» для КОМПАС-3D";
            this.Width = 520;
            this.Height = 320;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // --- Заголовок ---
            Label lblTitle = new Label();
            lblTitle.Text = "Параметры табуретки";
            lblTitle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // --- Поле H ---
            CreateParameterRow("Высота табуретки H:", "450", "мм", "400–500", 50, out txtH);

            // --- Поле W ---
            CreateParameterRow("Ширина сиденья W:", "400", "мм", "350–450", 85, out txtW);

            // --- Поле D ---
            CreateParameterRow("Глубина сиденья D:", "400", "мм", "350–450", 120, out txtD);

            // --- Поле d ---
            CreateParameterRow("Диаметр ножки d:", "40", "мм", "25–50", 155, out txtd);

            // --- Поле t ---
            CreateParameterRow("Толщина сиденья t:", "30", "мм", "20–40", 190, out txtt);

            // --- Кнопка «Построить» ---
            btnBuild = new Button();
            btnBuild.Text = "Построить";
            btnBuild.Font = new Font("Times New Roman", 11);
            btnBuild.Location = new Point(190, 235);
            btnBuild.Width = 130;
            btnBuild.Height = 32;
            btnBuild.Click += BtnBuild_Click;   // обработчик нажатия
            this.Controls.Add(btnBuild);
        }

        // ===== ВСПОМОГАТЕЛЬНЫЙ МЕТОД: СОЗДАТЬ СТРОКУ С ПАРАМЕТРОМ =====
        // Создаёт: подпись + поле ввода + единицу измерения + диапазон.
        private void CreateParameterRow(
            string labelText,
            string defaultValue,
            string unit,
            string range,
            int y,
            out TextBox textBox)
        {
            // Подпись
            Label lbl = new Label();
            lbl.Text = labelText;
            lbl.Font = new Font("Times New Roman", 11);
            lbl.Location = new Point(20, y + 4);
            lbl.AutoSize = true;
            this.Controls.Add(lbl);

            // Поле ввода
            textBox = new TextBox();
            textBox.Text = defaultValue;
            textBox.Font = new Font("Times New Roman", 11);
            textBox.Location = new Point(230, y);
            textBox.Width = 80;
            textBox.TextAlign = HorizontalAlignment.Right;
            this.Controls.Add(textBox);

            // Единица измерения
            Label lblUnit = new Label();
            lblUnit.Text = unit;
            lblUnit.Font = new Font("Times New Roman", 11);
            lblUnit.Location = new Point(320, y + 4);
            lblUnit.AutoSize = true;
            this.Controls.Add(lblUnit);

            // Диапазон
            Label lblRange = new Label();
            lblRange.Text = range;
            lblRange.Font = new Font("Times New Roman", 10, FontStyle.Italic);
            lblRange.ForeColor = Color.Gray;
            lblRange.Location = new Point(370, y + 4);
            lblRange.AutoSize = true;
            this.Controls.Add(lblRange);
        }

        // ===== ОБРАБОТЧИК КНОПКИ «ПОСТРОИТЬ» =====
        // Пока это заглушка — просто читает значения и показывает их.
        // В Лабе №4 здесь будет вызов builder.Build(parameters).
        private void BtnBuild_Click(object sender, EventArgs e)
        {
            // Сбросить подсветку
            txtH.BackColor = Color.White;
            txtW.BackColor = Color.White;
            txtD.BackColor = Color.White;
            txtd.BackColor = Color.White;
            txtt.BackColor = Color.White;

            // Объявляем переменные С НАЧАЛЬНЫМ ЗНАЧЕНИЕМ 0
            double H = 0, W = 0, D = 0, d = 0, t = 0;

            // Пытаемся преобразовать каждое поле по отдельности
            bool okH = double.TryParse(txtH.Text, out H);
            bool okW = double.TryParse(txtW.Text, out W);
            bool okD = double.TryParse(txtD.Text, out D);
            bool okd = double.TryParse(txtd.Text, out d);
            bool okt = double.TryParse(txtt.Text, out t);

            if (!okH || !okW || !okD || !okd || !okt)
            {
                MessageBox.Show("Введите числа во все поля.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка диапазонов
            bool hasError = false;
            if (H < 400 || H > 500) { txtH.BackColor = Color.LightPink; hasError = true; }
            if (W < 350 || W > 450) { txtW.BackColor = Color.LightPink; hasError = true; }
            if (D < 350 || D > 450) { txtD.BackColor = Color.LightPink; hasError = true; }
            if (d < 25 || d > 50) { txtd.BackColor = Color.LightPink; hasError = true; }
            if (t < 20 || t > 40) { txtt.BackColor = Color.LightPink; hasError = true; }

            // Проверка зависимости d >= H/12
            if (d < H / 12.0)
            {
                txtd.BackColor = Color.LightPink;
                hasError = true;
                MessageBox.Show(
                    $"Диаметр ножки должен быть не менее H/12 = {H / 12.0:F1} мм",
                    "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hasError)
            {
                MessageBox.Show("Некоторые параметры вне допустимого диапазона.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Если всё ок — показать заглушку
            MessageBox.Show(
                $"Параметры корректны:\nH={H}, W={W}, D={D}, d={d}, t={t}",
                "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}