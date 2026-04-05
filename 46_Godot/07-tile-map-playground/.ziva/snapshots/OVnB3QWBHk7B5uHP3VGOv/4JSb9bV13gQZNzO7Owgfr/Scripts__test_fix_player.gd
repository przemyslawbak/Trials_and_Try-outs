class_name TestFixPlayer extends SceneTree

func _init():
	var f = FileAccess.open("res://Scenes/player.tscn", FileAccess.WRITE)
	f.store_string("""[gd_scene load_steps=5 format=3 uid="uid://bdnfci5432o67"]

[ext_resource type="PackedScene" uid="uid://datokhdybmaop" path="res://Scenes/character_base.tscn" id="1_base"]
[ext_resource type="Script" uid="uid://kic7r2boipap" path="res://Scripts/player.gd" id="2_script"]
[ext_resource type="Texture2D" uid="uid://uuljhdmfsinj" path="res://Assets/Icons/foot-sign.png" id="3_foot"]
[ext_resource type="Texture2D" uid="uid://l5ys4vuski7d" path="res://Assets/Icons/sword-pngrepo-com.png" id="4_sword"]

[node name="Player" instance=ExtResource("1_base")]
script = ExtResource("2_script")

[node name="MoveTilesLabel" type="Label" parent="." index="2"]
z_index = 2
offset_left = -32.0
offset_top = -47.0
offset_right = -7.0
offset_bottom = -24.0
scale = Vector2(0.5, 0.5)
text = "3/3"
horizontal_alignment = 1

[node name="MoveTilesIcon" type="Sprite2D" parent="." index="3"]
z_index = 2
position = Vector2(-14, -41)
scale = Vector2(0.228431, 0.228431)
texture = ExtResource("3_foot")

[node name="SwordLabel" type="Label" parent="." index="4"]
z_index = 2
offset_left = 17.0
offset_top = -47.0
offset_right = 42.0
offset_bottom = -24.0
scale = Vector2(0.5, 0.5)
text = "3/3"
horizontal_alignment = 1

[node name="SwordIcon" type="Sprite2D" parent="." index="5"]
z_index = 2
position = Vector2(13, -41)
scale = Vector2(0.228431, 0.228431)
texture = ExtResource("4_sword")

[node name="Shield" type="Polygon2D" parent="." index="6"]
z_index = 3
position = Vector2(0, -41)
scale = Vector2(0.54905, 0.641267)
color = Color(0, 0.22363, 0.562309, 1)
polygon = PackedVector2Array(-9.40182, -10.2157, 9.5433, -10.2157, 9.5433, -0.143629, 7.98452, 7.89006, 0.310551, 12.6863, -7.84304, 8.01002, -9.40182, -0.0237236)
uv = PackedVector2Array(-9.40182, -10.2157, 9.5433, -10.2157, 9.5433, -0.143629, 7.98452, 7.89006, 0.310551, 12.6863, -7.84304, 8.01002, -9.52173, 0.0961819)
""")
	f.close()
	quit()
