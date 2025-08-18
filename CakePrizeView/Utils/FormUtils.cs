
namespace CakePrizeView.Utils
{
    public static class FormUtils
    {
        public static TextBox CreateTextBox(string txtName, int txtPrefix, int posX, int posY, int sizeX, int sizeY)
        {
            TextBox textBox = new TextBox();
            textBox.AcceptsTab = true;
            textBox.Font = new Font("Bookman Old Style", 9F, FontStyle.Bold);
            textBox.Location = new Point(posX, posY);
            textBox.Name = $"Txt_{txtName}_{txtPrefix}";
            textBox.Size = new Size(sizeX, sizeY);
            textBox.TabIndex = 1;
            textBox.Text = "250"; //replace
            return textBox;
        }

        public static Label CreateLabel(string labelName, int labelPrefix, int locationX, int locationY, int sizeX, int sizeY)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Bookman Old Style", 10F, FontStyle.Bold);
            label.Location = new Point(locationX, locationY);
            label.Name = $"Lbl_{labelName}_{labelPrefix}";
            label.Size = new Size(sizeX, sizeY);
            label.TabIndex = 0;
            label.Text = labelName;
            return label;
        }

        public static RadioButton CreateRadioButton(string rbtnText, int rbtnPrefix, int locationX, int locationY, int sizeX, int sizeY, bool isChecked = true)
        {
            RadioButton radioButton = new RadioButton();
            radioButton.AutoSize = true;
            radioButton.Checked = isChecked;
            radioButton.Location = new Point(locationX, locationY);
            radioButton.Name = $"Rbtn_{rbtnText}_{rbtnPrefix}";
            radioButton.Size = new Size(sizeX, sizeY);
            radioButton.TabIndex = 2;
            radioButton.TabStop = true;
            radioButton.Text = rbtnText;
            radioButton.TextAlign = ContentAlignment.BottomLeft;
            radioButton.UseVisualStyleBackColor = true;
            //radioButton.CheckedChanged += rbtnMin_CheckedChanged;
            return radioButton;
        }

        public static Panel CreateIngredientPanelRow(int rowNumber, Label ingredientName, TextBox ingredientTxt, 
            Label unitPrefix, RadioButton rbtnMin, RadioButton rbtnMax, int locationX, int locationY)
        {
            Panel PlIngredientRowPanel = new Panel();
            PlIngredientRowPanel.Controls.Add(ingredientName);
            PlIngredientRowPanel.Controls.Add(ingredientTxt);
            PlIngredientRowPanel.Controls.Add(unitPrefix);
            PlIngredientRowPanel.Controls.Add(rbtnMin);
            PlIngredientRowPanel.Controls.Add(rbtnMax);
            PlIngredientRowPanel.Location = new Point(locationX, locationY);
            PlIngredientRowPanel.Name = $"PlIngredientRowPanel_{rowNumber}";
            PlIngredientRowPanel.Size = new Size(464, 27);
            PlIngredientRowPanel.TabIndex = 11;
            return PlIngredientRowPanel;
        }
    }
}
