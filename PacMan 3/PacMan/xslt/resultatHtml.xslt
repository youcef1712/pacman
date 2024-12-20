<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:jeu="http://www.monjeu.com/jeuPacMan/sauvegarde">

    <!-- Template principal pour transformer le document -->
    <xsl:template match="/jeu:Sauvegarde">
        <html>
            <head>
                <title>Résultats des Joueurs</title>
                <link rel="stylesheet" type="text/css" href="../css/style.css"/>
            </head>
            <body>
                <div class="header">Résultats des Joueurs</div>

                <!-- Applique le template pour chaque joueur -->
                <xsl:apply-templates select="jeu:Joueur"/>
            </body>
        </html>
    </xsl:template>

    <!-- Template pour un joueur -->
    <xsl:template match="jeu:Joueur">
        <div class="sub-header">
            Joueur: <xsl:value-of select="jeu:Nom"/>
        </div>

        <table>
            <tr>
                <th>ID Partie</th>
                <th>Date</th>
                <th>Etat du Jeu</th>
                <th>Score</th>
            </tr>

            <!-- Applique le template pour chaque partie du joueur -->
            <xsl:apply-templates select="jeu:Parties/jeu:Partie"/>
        </table>

        <br/> <!-- Espacement entre les joueurs -->
    </xsl:template>

    <!-- Template pour une partie -->
    <xsl:template match="jeu:Partie">
        <tr>
            <td><xsl:value-of select="jeu:ID"/></td>
            <td><xsl:value-of select="jeu:Date"/></td>
            <td><xsl:value-of select="jeu:EtatJeu"/></td>
            <td><xsl:value-of select="jeu:Score"/></td>
        </tr>
    </xsl:template>

</xsl:stylesheet>
