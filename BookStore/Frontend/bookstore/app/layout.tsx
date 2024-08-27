import "./globals.css";
import Layout, { Content, Footer, Header } from "antd/es/layout/layout";
import { Menu } from "antd";
import Link from "next/link";

const items = [
  { key: "home", label: <Link href={"/"}>Главная</Link> },
  { key: "books", label: <Link href={"/books"}>Книги</Link> },
];

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Layout style={{ minHeight: "100vh" }}>
          <Menu
            mode="horizontal"
            items={items}
            style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              minWidth: 0,
            }}
          />
          <Content style={{ padding: "0 100px" }}>{children}</Content>
          <Footer style={{ fontSize: "16px", textAlign: "center" }}>
            Магазин книг
          </Footer>
        </Layout>
      </body>
    </html>
  );
}
